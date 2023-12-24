using System.Text.Json.Serialization;

namespace FeatBit.DataExport
{
    public class FlagValueEvent
    {
        public Object Id { get; set; }
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        public string EnvId { get; set; }
        [JsonPropertyName("FeatBitTimestamp")]
        public DateTime Timestamp { get; set; }
        public string VariationId { get; set; }
        public string DistinctId { get; set; }
        public string EventName { get; set; }
        public DateTime CHTimestamp { get; set; }
        public string Route { get; set; }
        public string FeatureFlagId { get; set; }
        public string AccountId { get; set; }
        public string ProjectId { get; set; }
        public string FeatureFlagKey { get; set; }
        public bool? SendToExperiment { get; set; }
        public string Username { get; set; }
        public string Tag0 { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
    }
}
