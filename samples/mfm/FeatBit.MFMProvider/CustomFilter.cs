using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace FeatBit.MFMProvider
{
    [FilterAlias("Custom")]
    public class CustomFilter: IFeatureFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            // get context parameter configuration, this are texts, so this can be the configuration of FeatBit
            // 

            var key = context.Parameters.GetSection("key").ToString();

            return true;
        }
    }
}
