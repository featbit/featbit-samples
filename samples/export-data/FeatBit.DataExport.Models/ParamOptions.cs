using CommandLine;

namespace FeatBit.DataExport
{
    public class ParamOptions
    {
        public ParamOptions()
        {
            QueryInterval = 1000;
            BigInterval = 30;
            PageSize = 1000;
            EventType = "FlagValue";
        }
        [Option('e', "env", Required = true, HelpText = "Environment where you want to query ")]
        public string EnvId { get; set; }

        [Option('t', "timestamp", Required = true, HelpText = "TimeStamp you want the query start with")]
        public string TimeStamp { get; set; }

        [Option('c', "conn", Required = true, HelpText = "Source Connection String to ClickHouse or MongoDB")]
        public string SourceConnectionString { get; set; }

        [Option('g', "segment", Required = false, HelpText = "Segment Connection String, example: WriteKey=NUc5S2Jl*****xOVN1eGhLQ29T**1SamRHWm4=;Host=https://api.segment.io")]
        public string SegmentConnectionString { get; set; }

        [Option('a', "azevthub", Required = false, HelpText = "Azure Event Hub Connection String")]
        public string AzureEventHubConnectionString { get; set; }

        [Option("azevthubplan", Required = false, HelpText = "Azure Event Hub Plan (Basic, Standard, Premium, Dedicated), this is useful for EventDataBatch item count")]
        public string AzureEventHubPlan { get; set; }

        [Option("azevthubname", Required = false, HelpText = "Azure Event Hub Name")]
        public string AzureEventHubName { get; set; }

        [Option('s', "size", Required = false, HelpText = "Page size should be greater than 0")]
        public int? PageSize { get; set; }

        [Option('i', "queryinterval", Required = false, HelpText = "Interval (milliseconds) between each query to FeatBit's ClickHouse or MongoDB")]
        public int QueryInterval { get; set; }

        [Option('b', "biginterval", Required = false, HelpText = "Interval (seconds) to next query if no items return from the query")]
        public int BigInterval { get; set; }

        [Option("eventtype", Required = false, HelpText = "The event type you want to export. 'FlagValue' for Feature Flag Evaluation or 'CustomEvent' for Metric")]
        public string EventType { get; set; }

        [Option("pipeline", Required = false, HelpText = "Options: FlagValueToSegment,FlagValueToAzureEventHub,CustomEventToSegment,CustomEventToAzureEventHub")]
        public string Pipeline { get; set; }
    }
}
