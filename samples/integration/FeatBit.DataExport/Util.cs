using CommandLine;

namespace FeatBit.DataExport
{
    public class Util
    {
        public static bool ParseParameters(string[] args, ParamOptions options)
        {
            Parser.Default
                    .ParseArguments<ParamOptions>(args)
                    .WithParsed<ParamOptions>(o =>
                    {
                        if (!string.IsNullOrWhiteSpace(o.EnvId))
                        {
                            options.EnvId = o.EnvId;
                        }
                        if (!string.IsNullOrWhiteSpace(o.TimeStamp))
                        {
                            options.TimeStamp = o.TimeStamp;
                        }
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
            if (!string.IsNullOrWhiteSpace(options.ConnectionString) &&
                !string.IsNullOrWhiteSpace(options.TimeStamp) &&
                !string.IsNullOrWhiteSpace(options.EnvId) &&
                options.PageSize > 0)
                return true;
            return false;
        }
    }
}
