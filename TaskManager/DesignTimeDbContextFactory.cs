using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using infrastructure.Persistence;
using DotNetEnv;

namespace TaskManager;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TaskManagerDbContext>
{
    public TaskManagerDbContext CreateDbContext(string[] args)
    {
        // Load environment variables
        Env.Load();

        // Get connection string from environment variable
        var connectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("MYSQL_CONNECTION_STRING environment variable is not set.");
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
