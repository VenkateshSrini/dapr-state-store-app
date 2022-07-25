namespace dapr.state.service.MessagePackets
{
    public class ResStoreDetails
    {
        public int StoreId { get; set; }
        public string Status { get; set; }
        public int StatuCode { get; set; }
        public string CacheStoreName { get; set; }
    }
}
