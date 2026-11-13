using Wip.Worker;
using Wip.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.Configure<CollectionSchedulerOptions>(builder.Configuration.GetSection("CollectionScheduler"));
builder.Services.AddScoped<ICollectionRunOrchestrator, CollectionRunOrchestrator>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
