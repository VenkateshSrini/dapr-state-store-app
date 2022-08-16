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
            //while (!stoppingToken.IsCancellationRequested)
            //{
            var stateStore = _configuration["StateStore"];

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var cacheObj = new EntityObject
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = $"Obj-{DateTime.Now}"
            };
            _logger.LogInformation($"Cache objected created with state store {stateStore}");
            try
            {
                bool saveResult = await _daprClient.TrySaveStateAsync<EntityObject>(stateStore, cacheObj.Id,
                cacheObj, "abc");
                _logger.LogInformation($"Cache save result {saveResult}");
                if (saveResult)
                {
                    _logger.LogInformation("*** Saved Cache Sucessfully****");
                    var newObj = await _daprClient.GetStateAsync<EntityObject>(stateStore, cacheObj.Id);
                    _logger.LogInformation($"Worker {newObj.Name}");
                }
                else
                    _logger.LogInformation("*** Saved Cache Failed****");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in state persistence {ex.Message}");
            }
            //}
        }
        
    }
}