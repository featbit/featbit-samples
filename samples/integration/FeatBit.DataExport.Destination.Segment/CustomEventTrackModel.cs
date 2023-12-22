using System.Text.Json.Serialization;

namespace FeatBit.DataExport.Destination.Segment
{
    public class CustomEventTrackModel : CustomEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        public CustomEventTrackModel(CustomEvent evt, string eventName = "FeatBit Custom Event")
        {
            CopyFromCustomEvent(evt);
            Event = eventName;
        }

        public void CopyFromCustomEvent(CustomEvent evt)
        {
            this.Id = evt.Id;
            this.UserId = evt.UserId;
            this.EnvId = evt.EnvId;
            this.Timestamp = evt.Timestamp;
            this.VariationId = evt.VariationId;
            this.DistinctId = evt.DistinctId;
            this.EventName = evt.EventName;
            this.CustomEventName = evt.CustomEventName;
            this.CHTimestamp = evt.CHTimestamp;
            this.Route = evt.Route;
            this.ApplicationType = evt.ApplicationType;
            this.AccountId = evt.AccountId;
            this.ProjectId = evt.ProjectId;
            this.NumericValue = evt.NumericValue;
            this.Username = evt.Username;
            this.Tag0 = evt.Tag0;
            this.Tag1 = evt.Tag1;
            this.Tag2 = evt.Tag2;
            this.Tag3 = evt.Tag3;
        }
    }
}
