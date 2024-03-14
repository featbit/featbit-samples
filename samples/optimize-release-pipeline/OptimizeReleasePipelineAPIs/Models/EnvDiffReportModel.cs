namespace OptimizeReleasePipelineAPIs.Models
{
    public class EnvDiffParam
    {
        public string LastEnvironmentId { get; set; }
        public string CurrentEnvironmentId { get; set; }
        // api-MDMwMDE3MDIzMDE3MQihrBYAofdU-nZ_jhQlruyA
        public string AccessToken { get; set; }
    }
    public class EnvDiffReportModel
    {
        public string? LastEnvironmentId { get; set; }
        public string? CurrentEnvironmentId { get; set; }
        public List<FeatureFlagSimple> FeatureFlags { get; set; }
        public List<string> Warnings { get; set; }
        public List<string> Errors { get; set; }
    }
}
