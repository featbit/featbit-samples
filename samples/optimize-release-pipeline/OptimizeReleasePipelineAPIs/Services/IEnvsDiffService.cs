using OptimizeReleasePipelineAPIs.Models;

namespace OptimizeReleasePipelineAPIs.Services
{
    public interface IEnvsDiffService
    {
        Task<EnvDiffReportModel> FindKeyAndVariationDiffAsync(EnvDiffParam param);
    }
}
