using Segment.Analytics.Utilities;

namespace FeatBit.DataExport.CHToSegment
{
    public class ProxyHttpClient : DefaultHTTPClient
    {
        public ProxyHttpClient(string apiKey, string apiHost = null, string cdnHost = null) : base(apiKey, apiHost, cdnHost)
        {
        }

        public override string SegmentURL(string host, string path)
        {
            if (host.Equals(_apiHost))
            {
                return "Your proxy api url";
            }
            else
            {
                return "Your proxy cdn url";
            }
        }
    }

    public class ProxyHttpClientProvider : IHTTPClientProvider
    {
        public HTTPClient CreateHTTPClient(string apiKey, string apiHost = null, string cdnHost = null)
        {
            return new ProxyHttpClient(apiKey, apiHost, cdnHost);
        }
    }
}
