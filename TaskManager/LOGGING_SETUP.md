# Loki & Grafana Logging Integration

This document explains the logging setup for the TaskManager application using Loki for log aggregation and Grafana for visualization.

## What Was Configured

### 1. Docker Services
- **Loki**: Log aggregation system running on port 3100
- **Grafana**: Visualization platform running on port 3000
- **MySQL**: Existing database service (unchanged)

### 2. Application Logging
- **Serilog**: Structured logging with multiple enrichers
- **Loki Sink**: Direct log shipping to Loki
- **Request Logging**: Comprehensive HTTP access logs
- **Error Logging**: Automatic error and exception capture

### 3. Monitoring Dashboard
- HTTP request rates and response times
- Error tracking and counts
- Status code distribution
- Real-time log viewing

## Getting Started

### 1. Configure Environment Variables
The application uses environment variables for logging configuration. These are already set in the `.env` file:

```bash
# Logging settings
LOKI_URL="http://localhost:3100"     # Loki server URL
APP_NAME="task-manager"              # Application name for log labeling
LOG_LEVEL="Information"              # Minimum log level (Debug, Information, Warning, Error, Fatal)
```

You can modify these values in the `.env` file or set them as system environment variables.

**Environment Variables:**
- `LOKI_URL`: The URL where Loki is running (default: "http://localhost:3100")
- `APP_NAME`: The application name used for log labeling (default: "task-manager")
- `LOG_LEVEL`: Minimum log level - Debug, Information, Warning, Error, Fatal (default: "Information")

**For Production:**
```bash
# Production example
LOKI_URL="https://loki.your-domain.com"
APP_NAME="task-manager-prod"
LOG_LEVEL="Warning"
```

### 2. Start the Infrastructure
```bash
cd TaskManager
docker-compose -f docker-compose.dev.yaml up -d
```

This will start:
- MySQL on port 3306
- Loki on port 3100
- Grafana on port 3000

### 3. Install NuGet Packages
```bash
dotnet restore
```

The following packages were added:
- Serilog.AspNetCore
- Serilog.Sinks.Grafana.Loki
- Serilog.Enrichers.Environment
- Serilog.Enrichers.Process
- Serilog.Enrichers.Thread

### 4. Run Your Application
```bash
dotnet run
```

### 5. Access Grafana
- URL: http://localhost:3000
- Username: `admin`
- Password: `admin`

The dashboard "TaskManager Application Monitoring" will be automatically provisioned.

## What Gets Logged

### Access Logs
- HTTP method, path, status code
- Response time
- User agent, remote IP
- Request host and scheme

### Error Logs
- Application exceptions
- Fatal errors
- Structured error context

### Application Logs
- Startup/shutdown events
- Custom application events
- Database operations
- Service layer activities

## Dashboard Features

### 1. HTTP Request Rate
Real-time chart showing request volume over time

### 2. Error Count Gauge
Current error count in the last hour

### 3. HTTP Status Codes
Distribution of response status codes (200, 404, 500, etc.)

### 4. Response Time (95th percentile)
Performance monitoring with 95th percentile response times

### 5. Error Rate
Rate of errors and fatal exceptions

### 6. Error Logs Panel
Real-time view of error and fatal log entries

### 7. Access Logs Panel
Real-time view of HTTP access logs

## Log Labels

Logs are automatically tagged with:
- `app`: "task-manager"
- `environment`: Development/Production
- `machine_name`: Server hostname
- `process_id`: Application process ID
- `thread_id`: Execution thread ID

## Querying Logs

### Basic Queries
```logql
# All application logs
{app="task-manager"}

# Only errors
{app="task-manager"} |= "ERROR"

# HTTP requests
{app="task-manager"} |= "HTTP"

# Specific status codes
{app="task-manager"} |= "HTTP" |= "500"
```

### Advanced Queries
```logql
# Request rate
rate({app="task-manager"} |= "HTTP" [5m])

# Error rate
rate({app="task-manager"} |= "ERROR" [5m])

# Response time extraction
{app="task-manager"} |= "HTTP" 
| regexp "in (?P<elapsed>\\d+\\.\\d+) ms" 
| unwrap elapsed 
| quantile_over_time(0.95, [5m])
```

## Troubleshooting

### Logs Not Appearing
1. Check if Loki is running: `docker ps`
2. Verify Loki logs: `docker logs loki`
3. Check application logs for Serilog errors

### Grafana Dashboard Not Loading
1. Verify datasource configuration in Grafana
2. Check if Loki is accessible from Grafana container
3. Review Grafana logs: `docker logs grafana`

### Application Startup Issues
1. Ensure all NuGet packages are installed
2. Check for compilation errors
3. Verify environment variables if any are required

## Log Retention

By default, Loki stores logs locally in the container. For production:
1. Configure external storage (S3, GCS, etc.)
2. Set up log retention policies
3. Consider log rotation strategies

## Security Considerations

For production deployment:
1. Change default Grafana admin password
2. Configure authentication for Grafana
3. Set up proper network security
4. Consider log data sensitivity
