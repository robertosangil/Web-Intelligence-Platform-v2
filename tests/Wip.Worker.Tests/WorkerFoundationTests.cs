using Microsoft.Extensions.Hosting;

namespace Wip.Worker.Tests;

public sealed class WorkerFoundationTests
{
    [Fact]
    public void WorkerIsAnIndependentBackgroundService()
    {
        Assert.True(typeof(global::Wip.Worker.Worker).IsAssignableTo(typeof(BackgroundService)));
    }
}
