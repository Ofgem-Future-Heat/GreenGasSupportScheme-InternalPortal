namespace InternalPortal.Configuration
{
    public class ApiConfig
    {
        public int RetryCount { get; set; }
        public double RetryIntervalSeconds { get; set; }
        public string InternalApiBaseUri { get; set; }
        public string DocumentsApiBaseUri { get; set; }
    }
}