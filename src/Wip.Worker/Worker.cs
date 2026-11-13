using Microsoft.Extensions.Options;

namespace Wip.Worker;

public class Worker(
    ILogger<Worker> logger,
    IOptions<CollectionSchedulerOptions> options,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("WIP background worker started.");

        try
        {
            var scheduler = options.Value;
            if (!scheduler.Enabled)
            {
                logger.LogInformation("Collection scheduler is disabled by configuration.");
                await Task.Delay(Timeout.InfiniteTimeSpan, stoppingToken);
                return;
            }

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Math.Max(5, scheduler.PollIntervalSeconds)));
            var lastRuns = new Dictionary<string, DateTimeOffset>();
            var lastRunsLock = new object();
            do
            {
                var due = CollectionSchedulePlanner.GetDue(scheduler.Definitions, DateTimeOffset.UtcNow, lastRuns);
                using var semaphore = new SemaphoreSlim(Math.Max(1, scheduler.MaxConcurrency));
                var tasks = due.Select(async definition =>
                {
                    await semaphore.WaitAsync(stoppingToken);
                    try
                    {
                        await using var scope = scopeFactory.CreateAsyncScope();
                        var orchestrator = scope.ServiceProvider.GetRequiredService<ICollectionRunOrchestrator>();
                        await orchestrator.CreateRunAsync(definition, stoppingToken);
                        lock (lastRunsLock)
                        {
                            lastRuns[CollectionSchedulePlanner.Key(definition)] = DateTimeOffset.UtcNow;
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                await Task.WhenAll(tasks);
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("WIP background worker stopping.");
        }
    }
}
