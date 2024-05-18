using FeatBit.Sdk.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace FeatBit.MFMProvider
{
    [FilterAlias("FeatBit")]
    public class FeatBitFilter : IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFbClient _fbClient;

        public FeatBitFilter(IHttpContextAccessor httpContextAccessor, IFbClient fbClient)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _fbClient = fbClient;
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var key = context.Parameters.GetSection("key").ToString();

            return true;
        }

    }
}
