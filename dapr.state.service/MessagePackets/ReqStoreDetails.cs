using System.Security.Cryptography.X509Certificates;

namespace dapr.state.service.MessagePackets
{
    public class ReqStoreDetails
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreLocation { get; set; }
        public float StoreRent { get; set; }
        public string ETagHash()
        {
            return $"{StoreName}-{StoreLocation}".GetHashCode().ToString();
    }
    }
    
}
