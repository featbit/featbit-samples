using System.Text.Json.Serialization;

namespace FeatBit.DataExport.Destination.Segment
{
    public class FlagValueTrackModel: FlagValueEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        public FlagValueTrackModel(FlagValueEvent evt, string eventName = "Feature Flag Evaluation Event")
        {
            CopyFromFlagValueEvent(evt);
            Event = eventName;
        }

        public void CopyFromFlagValueEvent(FlagValueEvent evt)
        {
            this.Id = evt.Id;
            this.UserId = evt.UserId;
            this.EnvId = evt.EnvId;
            this.Timestamp = evt.Timestamp;
            this.VariationId = evt.VariationId;
            this.DistinctId = evt.DistinctId;
            this.EventName = evt.EventName;
            this.CHTimestamp = evt.CHTimestamp;
            this.Route = evt.Route;
            this.FeatureFlagId = evt.FeatureFlagId;
            this.AccountId = evt.AccountId;
            this.ProjectId = evt.ProjectId;
            this.FeatureFlagKey = evt.FeatureFlagKey;
            this.SendToExperiment = evt.SendToExperiment;
            this.Username = evt.Username;
            this.Tag0 = evt.Tag0;
            this.Tag1 = evt.Tag1;
            this.Tag2 = evt.Tag2;
            this.Tag3 = evt.Tag3;
        }
    }
}
