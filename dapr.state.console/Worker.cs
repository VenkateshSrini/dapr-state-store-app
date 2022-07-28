using Dapr.Client;

namespace dapr.state.console
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DaprClient _daprClient;
        public Worker(ILogger<Worker> logger, DaprClient daprClient )
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            if (_daprClient != null)
                _daprClient.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}