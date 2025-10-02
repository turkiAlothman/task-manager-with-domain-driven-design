using Microsoft.Extensions.Diagnostics.HealthChecks;
using infrastructure.Services;
using Microsoft.Extensions.Options;
using infrastructure.Configurations;

namespace infrastructure.HealthChecks
{
    public class MinioHealthCheck : IHealthCheck
    {
        private readonly IMinioService _minioService;
        private readonly MinioSettings _minioSettings;

        public MinioHealthCheck(IMinioService minioService, IOptions<MinioSettings> minioSettings)
        {
            _minioService = minioService;
            _minioSettings = minioSettings.Value;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Try to ensure bucket exists (this will verify MinIO connectivity and create bucket if needed)
                await _minioService.EnsureBucketExistsAsync();
                
                return HealthCheckResult.Healthy($"MinIO is healthy. Bucket '{_minioSettings.BucketName}' exists and is accessible.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"MinIO health check failed: {ex.Message}", ex);
            }
        }
    }
}
