using Wip.Worker;
using Wip.Persistence;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
