using Microsoft.AspNetCore.Mvc;
using OptimizeReleasePipelineAPIs.Models;
using OptimizeReleasePipelineAPIs.Services;

namespace OptimizeReleasePipelineAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnvsDiffController : ControllerBase
    {
        private readonly ILogger<EnvsDiffController> _logger;
        private readonly IEnvsDiffService _envsDiffService;

        public EnvsDiffController(
            ILogger<EnvsDiffController> logger,
            IEnvsDiffService envsDiffService)
        {
            _logger = logger;
            _envsDiffService = envsDiffService;
        }

        [HttpPost]
        public async Task<EnvDiffReportModel> CompareEnvsDiff([FromBody] EnvDiffParam param)
        {
            param.LastEnvironmentId = "336c6c47-5974-48e2-8f87-e4cfbb114886";
            param.CurrentEnvironmentId = "99d5be11-5a33-4172-ad8b-0f3c18a12314";
            param.AccessToken = "api-MDMwMDE3MDIzMDE3MQihrBYAofdU-nZ_jhQlruyA";
            return await _envsDiffService.FindKeyAndVariationDiffAsync(param);
        }
    }


}
