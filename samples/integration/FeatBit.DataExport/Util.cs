using CommandLine;

namespace FeatBit.DataExport
{
    public class Util
    {
        public static bool ParseParameters(string[] args, ParamOptions options)
        {
            //Console.WriteLine(String.Join(';', args));
            Parser.Default
                    .ParseArguments<ParamOptions>(args)
                    .WithParsed<ParamOptions>(o =>
                    {
                        options.EnvId = o.EnvId;
                        options.TimeStamp = o.TimeStamp;
                        options.SourceConnectionString = o.SourceConnectionString;
                        options.QueryInterval = o.QueryInterval;
                        options.BigInterval = o.BigInterval == 0 ? 30 : o.BigInterval;

                        options.SegmentConnectionString = o.SegmentConnectionString;
                        options.AzureEventHubConnectionString = o.AzureEventHubConnectionString;
                        options.AzureEventHubPlan = o.AzureEventHubPlan;
                        options.AzureEventHubName = o.AzureEventHubName;
                        options.EventType = o.EventType;

                        if (o.PageSize != null)
                        {
                            if (o.PageSize > 0)
                            {
                                options.PageSize = o.PageSize.Value;
                            }
                            else
                            {
                                Console.WriteLine($"Page Size should greater than 0");
                            }
                        }
                    });
            if (!string.IsNullOrWhiteSpace(options.SourceConnectionString) &&
                !string.IsNullOrWhiteSpace(options.TimeStamp) &&
                !string.IsNullOrWhiteSpace(options.EnvId) &&
                options.PageSize > 0)
                return true;
            return false;
        }
    }
}
