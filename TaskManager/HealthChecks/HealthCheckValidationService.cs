using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace TaskManager.HealthChecks
{
    public static class HealthCheckValidationService
    {
        public static async Task ValidateHealthChecksAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            logger.LogInformation("Performing health checks before starting application...");
            
            using var scope = serviceProvider.CreateScope();
            var healthCheckService = scope.ServiceProvider.GetRequiredService<HealthCheckService>();
            
            var healthCheckResult = await healthCheckService.CheckHealthAsync();
            
            LogHealthCheckResults(logger, healthCheckResult);
            
            HandleHealthCheckResults(logger, healthCheckResult);
        }
        
        private static void LogHealthCheckResults(ILogger logger, HealthReport healthCheckResult)
        {
            logger.LogInformation("=== HEALTH CHECK RESULTS ===");
            logger.LogInformation($"Overall Status: {healthCheckResult.Status}");
            logger.LogInformation($"Total Duration: {healthCheckResult.TotalDuration.TotalMilliseconds:F2}ms");
            
            foreach (var entry in healthCheckResult.Entries)
            {
                var name = entry.Key;
                var result = entry.Value;
                
                var statusIcon = result.Status switch
                {
                    HealthStatus.Healthy => "✅",
                    HealthStatus.Degraded => "⚠️ ",
                    HealthStatus.Unhealthy => "❌",
                    _ => "❓"
                };
                
                logger.LogInformation($"{statusIcon} {name}: {result.Status} ({result.Duration.TotalMilliseconds:F2}ms)");
                
                if (!string.IsNullOrEmpty(result.Description))
                {
                    logger.LogInformation($"   Description: {result.Description}");
                }
                
                if (result.Exception != null)
                {
                    logger.LogWarning($"   Exception: {result.Exception.Message}");
                }
            }
            
            logger.LogInformation("=== END HEALTH CHECK RESULTS ===");
        }
        
        private static void HandleHealthCheckResults(ILogger logger, HealthReport healthCheckResult)
        {
            // Define critical services that must be healthy for the application to start
            var criticalChecks = new[] { "mysql", "redis" };
            
            var failedCriticalChecks = healthCheckResult.Entries
                .Where(entry => criticalChecks.Contains(entry.Key) && entry.Value.Status == HealthStatus.Unhealthy)
                .ToList();
            
            if (failedCriticalChecks.Any())
            {
                var failedServices = string.Join(", ", failedCriticalChecks.Select(entry => entry.Key));
                logger.LogCritical($"Critical health checks failed: {failedServices}. Application cannot start.");
                logger.LogCritical("Please ensure all critical dependencies are running and accessible.");
                
                // Exit the application if critical services are not available
                Environment.Exit(1);
            }
            
            // Log warnings for non-critical failed services
            var failedNonCriticalChecks = healthCheckResult.Entries
                .Where(entry => !criticalChecks.Contains(entry.Key) && entry.Value.Status == HealthStatus.Unhealthy)
                .ToList();
            
            if (failedNonCriticalChecks.Any())
            {
                var failedServices = string.Join(", ", failedNonCriticalChecks.Select(entry => entry.Key));
                logger.LogWarning($"Non-critical health checks failed: {failedServices}. Application will start but some features may not work correctly.");
            }
            
            if (healthCheckResult.Status == HealthStatus.Healthy)
            {
                logger.LogInformation("All health checks passed successfully! Starting application...");
            }
            else if (healthCheckResult.Status == HealthStatus.Degraded)
            {
                logger.LogWarning("Some health checks are degraded, but application will start...");
            }
        }
    }
}
