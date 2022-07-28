using dapr.state.console.Model;
using Dapr.Client;

namespace dapr.state.console
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DaprClient _daprClient;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, DaprClient daprClient,
            IConfiguration configuration)
        {
            _logger = logger;
            _daprClient = daprClient;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var stateStore = _configuration["State.Store"];
                
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                var cacheObj = new EntityObject
                {
                    Id = System.Guid.NewGuid().ToString(),
                    Name = $"Obj-{DateTime.Now}"
                };
                await _daprClient.SaveStateAsync<EntityObject>(stateStore, cacheObj.Id,
                    cacheObj);
                var newObj = await _daprClient.GetStateAsync<EntityObject>(stateStore, cacheObj.Id);
                _logger.LogInformation($"Worker {newObj.Name}");
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