using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using infrastructure.Persistence;
using DotNetEnv;

namespace TaskManager;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
{
    public TaskManagerDbContext CreateDbContext(string[] args)
    {
        // Set environment variable to signal that we're in design-time mode
        // This prevents Program.cs from running its full startup sequence
        Environment.SetEnvironmentVariable("EF_DESIGN_TIME", "true");
        
        // First, try to get connection string from environment variable (for Docker/production)
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        
        // If not found in environment variables, load from .env file (for local development)
        if (string.IsNullOrEmpty(connectionString))
        {
            var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
            if (File.Exists(envFilePath))
            {
                Env.Load();
                connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
            }
        }
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "MYSQL_CONNECTION_STRING is not set. " +
                "Please set it as an environment variable or add it to the .env file.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<TaskManagerDbContext>();
        optionsBuilder.UseMySql(
            connectionString, 
            ServerVersion.AutoDetect(connectionString),
            options => options.MigrationsAssembly("TaskManager").CommandTimeout(50)
        );

        return new TaskManagerDbContext(optionsBuilder.Options);
    }
}
