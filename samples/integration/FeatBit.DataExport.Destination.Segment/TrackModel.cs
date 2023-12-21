using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FeatBit.DataExport.Destination.Segment
{
    public class TrackModel: FlagValueEvent
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        //[JsonPropertyName("timestamp")]
        //public DateTime SegmentTimeStamp { get; set; }

        public TrackModel(FlagValueEvent evt, string eventName = "Feature Flag Evaluation Event")
        {
            CopyFromFlagValueEvent(evt);
            Event = eventName;
            //SegmentTimeStamp = evt.Timestamp;
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
