using dapr.state.console;
using dapr.state.console.extensions.service;
using System.Diagnostics;

var stateStore = string.Empty;
var isDebugging = Debugger.IsAttached;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configBuilder =>
    {
        configBuilder.AddJsonFile("appsettings.json");
    })
    .ConfigureAppConfiguration((hostBuilderCtx, configBuilderCtx) => {
        configBuilderCtx.AddEnvironmentVariables();
        if (args is { Length: > 0 })
        {
            configBuilderCtx.AddCommandLine(args);
        }
        stateStore = hostBuilderCtx.Configuration["StateStore"];
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
