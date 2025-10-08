using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using TaskManager.Middlewares;
using Application.Services.Interfaces;
using infrastructure;
using infrastructure.Persistence.Repositores;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using TaskManager.Services;
using infrastructure.Configurations;
using TaskManager;
using infrastructure.Persistence;
using infrastructure;
using Domain.DomainModels.Activitiy;
using Domain.DomainModels.Comment;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.DomainModels.Team;
using DotNetEnv;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using Hangfire;
using Hangfire.Redis.StackExchange;
using infrastructure.HealthChecks;
using TaskManager.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TaskManager.Configuration;

// Detect if running in design-time mode (EF migrations)
// Check multiple indicators that EF tools are running this
var isEfDesignTime = args.Any(a => a.StartsWith("--applicationName")) ||
                     args.Any(a => a.Contains("Microsoft.EntityFrameworkCore")) ||
                     Environment.GetEnvironmentVariable("EF_DESIGN_TIME") == "true" ||
                     AppDomain.CurrentDomain.GetData("EF.Design.IsDesignTime") != null;

if (isEfDesignTime)
{
    // Design-time mode detected - exit without running the application
    // The DesignTimeDbContextFactory will handle database operations
    return;
}

Env.Load();


// check the existance of environment variables
StartUpChecks.CheckEnvironmentsExistance();

// Configure Serilog
var lokiUrl = Environment.GetEnvironmentVariable("LOKI_URL") ?? "http://localhost:3100";
var appName = Environment.GetEnvironmentVariable("APP_NAME") ?? "task-manager";
var logLevel = Environment.GetEnvironmentVariable("LOG_LEVEL") ?? "Information";

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var isProduction = environment.Equals("Production", StringComparison.OrdinalIgnoreCase);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Is(Enum.Parse<LogEventLevel>(logLevel))
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", isProduction ? LogEventLevel.Warning : LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", isProduction ? LogEventLevel.Warning : LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithMachineName()
    .Enrich.WithProcessId()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(lokiUrl, labels: new List<LokiLabel>
    {
        new() { Key = "app", Value = appName },
        new() { Key = "environment", Value = environment }
    })
    .CreateLogger();

try
{
    Log.Information("Starting TaskManager application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog to the DI container
    builder.Host.UseSerilog();

    // Health checks configuration
    var mysqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING") ?? builder.Configuration["MYSQL_CONNECTION_STRING"];
    var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost:6379";

    builder.Services.AddHealthChecks()
        .AddMySql(mysqlConnectionString, name: "mysql", failureStatus: HealthStatus.Unhealthy, tags: new[] { "db", "mysql" })
        .AddRedis(redisConnectionString, name: "redis", failureStatus: HealthStatus.Unhealthy, tags: new[] { "cache", "redis" })
        .AddCheck<MinioHealthCheck>("minio", failureStatus: HealthStatus.Unhealthy, tags: new[] { "storage", "minio" })
        .AddCheck<LokiHealthCheck>("loki", failureStatus: HealthStatus.Unhealthy, tags: new[] { "logging", "loki" });

    // Register custom health check dependencies
    builder.Services.AddScoped<MinioHealthCheck>();
    builder.Services.AddHttpClient<LokiHealthCheck>();

    builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
        .AddEnvironmentVariables();

    // mvc configurations with json respone + NewtonsoftJson (for patial update in patch reuqest)
    builder.Services.AddControllersWithViews(c=>c.Filters.Add(new AuthorizeFilter()))
        .AddNewtonsoftJson(Options => Options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
        .AddJsonOptions(Options => Options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

    // authentication
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(e=>e.LoginPath="/auth/login");

    // add repositories
    builder.Services.AddRepositories();

    // email configurations 
    builder.Services.Configure<MailSettings>(options =>
    {
        options.Server = Environment.GetEnvironmentVariable("MAIL_SERVER");
        options.Username = Environment.GetEnvironmentVariable("MAIL_USERNAME");
        options.Password = Environment.GetEnvironmentVariable("MAIL_PASSWORD");
        options.SenderName = Environment.GetEnvironmentVariable("MAIL_SENDER_NAME");
        options.Port = int.TryParse(Environment.GetEnvironmentVariable("MAIL_PORT"), out var port) ? port : 25;
        options.TLS = bool.TryParse(Environment.GetEnvironmentVariable("MAIL_TLS"), out var tls) && tls;
    });

    // Hangfire configuration with Redis storage
    builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseRedisStorage(redisConnectionString));

    builder.Services.AddHangfireServer();

    // MinIO configuration
    builder.Services.Configure<infrastructure.Configurations.MinioSettings>(options =>
    {
        options.Endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT") ?? "localhost:9000";
        options.AccessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY") ?? "minioadmin";
        options.SecretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY") ?? "minioadmin123";
        options.BucketName = Environment.GetEnvironmentVariable("MINIO_BUCKET_NAME") ?? "task-attachments";
        options.UseSSL = bool.TryParse(Environment.GetEnvironmentVariable("MINIO_USE_SSL"), out var useSSL) && useSSL;
        options.PresignedUrlExpiry = int.TryParse(Environment.GetEnvironmentVariable("MINIO_PRESIGNED_URL_EXPIRY"), out var expiry) ? expiry : 604800;
    });

    // Custom services
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddApplicationServices();
    builder.Services.AddMinioServices(builder.Configuration);

    // configurting the database(mysql)
    string? connectionString = builder.Configuration["MYSQL_CONNECTION_STRING"];
    builder.Services.AddDbContext<TaskManagerDbContext>(Options=>{
        var dbOptions = Options.UseMySql(
            connectionString, ServerVersion.AutoDetect(connectionString),
            options => options.MigrationsAssembly("TaskManager").CommandTimeout(50)
        );
        
        // Only enable detailed logging in Development
        if (builder.Environment.IsDevelopment())
        {
            dbOptions.EnableSensitiveDataLogging()
                     .EnableDetailedErrors();
        }
        else
        {
            // Production: Disable all query logging and sensitive data
            dbOptions.EnableSensitiveDataLogging(false)
                     .EnableDetailedErrors(false)
                     .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting));
        }
    });

    // swagger configurations 
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //  register httpContext as service
    builder.Services.AddHttpContextAccessor();

    // set Multipart Body Length Limit to 10MB
    builder.Services.Configure<FormOptions>(Options => Options.MultipartBodyLengthLimit = 10000000);

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
            diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString());
        };
    });

    app.UseStaticFiles();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Health check endpoints
    app.UseHealthChecks("/health");
    app.UseHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        Predicate = _ => true
    });
    app.UseHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
    {
        Predicate = _ => false
    });

    app.UseAuthentication();

    // Add Hangfire Dashboard (only in development)
    if (app.Environment.IsDevelopment())
    {
        app.UseHangfireDashboard("/hangfire");
    }

    // middleware for inserting tasks activites in the database
    app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/Tasks") && 
        !context.Request.Method.Equals("GET") && context.Response.StatusCode == 200, 
        AppBuilder => AppBuilder.UseMiddleware<TasksActivitiesMiddleware>());

    app.UseExceptionHandler("/error");

    app.MapControllerRoute(name:"default",pattern:"{controller=Home}/{action=dashboard}/{id?}");

    // insert dummay data
    if (app.Environment.IsDevelopment())
       DbSeeder.Seed(app);

    // Manual health check validation before starting the application
    var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("HealthCheckValidation");
    await HealthCheckValidationService.ValidateHealthChecksAsync(app.Services, logger);

    Log.Information("TaskManager application started successfully");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
