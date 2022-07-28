using dapr.state.console;
using dapr.state.console.extensions.service;
using System.Diagnostics;

var stateStore = string.Empty;
var isDebugging = Debugger.IsAttached;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configBuilder =>
    {
        configBuilder.AddJsonFile("appsettings.json")
                     .AddEnvironmentVariables();
        if (args is { Length: > 0 })
        {
            configBuilder.AddCommandLine(args);
        }
    })
    .ConfigureAppConfiguration((hostBuilderCtx, configBuilderCtx) => {
        stateStore = hostBuilderCtx.Configuration["State.Store"];
    })
    .ConfigureServices(services =>
    {
        services.AddDaprClient();
        services.AddHostedService<Worker>();
    })
    .Build();
if (isDebugging)
    await host.RunAsync();
else
{
    using (host)
    {
        await host.StartAsync();
        await host.StopAsync();
    }
}
