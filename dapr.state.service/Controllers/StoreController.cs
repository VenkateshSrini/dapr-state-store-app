using dapr.state.service.MessagePackets;
using Dapr.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dapr.state.service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly DaprClient? _daprClient = default;
        private readonly ILogger<StoreController> _logger;
        private readonly IConfiguration _configuration;
        public StoreController(DaprClient daprClient, 
            ILogger<StoreController> logger,
            IConfiguration configuration)
        {
            _daprClient = daprClient;
            _logger = logger;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<ActionResult<ReqStoreDetails>>Get(int storeId)
        {
            var storeName = _configuration["State.Store"];
            var appName = Environment.GetEnvironmentVariable("app-id");
            var key = (!string.IsNullOrEmpty(appName)) ? $"{appName}||{storeId}"
                                                      : storeId.ToString();

            var storeDetails = await _daprClient?.GetStateEntryAsync<ReqStoreDetails>(storeName,
                key);
            return Ok(storeDetails?.Value);

        }
        [HttpPost]
        public async Task<ActionResult<ResStoreDetails>>Post(ReqStoreDetails storeDetails)
        {
            var storeName = _configuration["State.Store"];
           bool saveResult= await _daprClient?.TrySaveStateAsync<ReqStoreDetails>(storeName, storeDetails.StoreId.ToString(),
                storeDetails, storeDetails.ETagHash());
            if (saveResult)
                return new OkObjectResult(new ResStoreDetails
                {
                    StoreId = storeDetails.StoreId,
                    CacheStoreName = storeName,
                    StatuCode = 200,
                    Status = "success"
                });
            else
                return new BadRequestObjectResult(new ResStoreDetails
                {
                    StoreId = storeDetails.StoreId,
                    CacheStoreName = storeName,
                    StatuCode = 400,
                    Status = "BadRequest"
                });

        }
    }
}
