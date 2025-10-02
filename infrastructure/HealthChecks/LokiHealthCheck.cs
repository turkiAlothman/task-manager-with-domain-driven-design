using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace infrastructure.HealthChecks
{
    public class LokiHealthCheck : IHealthCheck
    {
        private readonly HttpClient _httpClient;
        private readonly string _lokiUrl;

        public LokiHealthCheck(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _lokiUrl = Environment.GetEnvironmentVariable("LOKI_URL") ?? configuration["LOKI_URL"] ?? "http://localhost:3100";
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check Loki's ready endpoint
                var response = await _httpClient.GetAsync($"{_lokiUrl}/ready", cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken);
                    return HealthCheckResult.Healthy($"Loki is healthy and ready. Response: {content}");
                }
                else
                {
                    return HealthCheckResult.Unhealthy($"Loki health check failed. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                }
            }
            catch (HttpRequestException ex)
            {
                return HealthCheckResult.Unhealthy($"Loki health check failed: Unable to connect to Loki at {_lokiUrl}. {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                return HealthCheckResult.Unhealthy($"Loki health check timed out: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Loki health check failed: {ex.Message}", ex);
            }
        }
    }
}
