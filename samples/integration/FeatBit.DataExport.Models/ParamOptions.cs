using CommandLine;

namespace FeatBit.DataExport
{
    public class ParamOptions
    {
        [Option('e', "env", Required = true, HelpText = "Please input env id")]
        public string EnvId { get; set; }

        [Option('t', "timestamp", Required = true, HelpText = "Please input timestamp")]
        public string TimeStamp { get; set; }

        [Option('s', "size", Required = false, HelpText = "Page size should be greater than 0")]
        public int? PageSize { get; set; }

        [Option('c', "conn", Required = true, HelpText = "Please input connection string")]
        public string ConnectionString { get; set; }

        [Option('d', "destination", Required = true, HelpText = "Please input a destination")]
        public string Destination { get; set; }
    }
}
