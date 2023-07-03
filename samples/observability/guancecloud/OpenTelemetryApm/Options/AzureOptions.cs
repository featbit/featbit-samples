namespace OpenTelemetryApm.Options
{
    public class AzureOptions
    {
        public const string Position = "Azure";

        public RedisCacheOptions RedisCache { get; set; } = new RedisCacheOptions();
    }

    public class RedisCacheOptions
    {

        public const string Position = "RedisCache";

        public string ConnectionString { get; set; } = String.Empty;
        public string InstanceName { get; set; } = String.Empty;
    }
}
