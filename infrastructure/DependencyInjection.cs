using Application.Services.Interfaces;
using Domain.Base;
using Domain.DomainModels.Activitiy;
using Domain.DomainModels.Comment;
using Domain.DomainModels.Employee;
using Domain.DomainModels.ResetPasswords;
using Domain.DomainModels.Task;
using Domain.DomainModels.Team;
using infrastructure.Configurations;
using infrastructure.Persistence.Repositores;
using infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection collection)
        {
            // services
            collection.AddTransient<IEmailService, EmailService>();
            collection.AddTransient<ITasksService, TasksService>();
            collection.AddTransient<IEmployeesService, EmployeesService>();
            collection.AddTransient<IProjectsService, ProjectsService>();
            collection.AddTransient<IInviteService, InviteService>();
            collection.AddTransient<IResetPasswordService, ResetPasswordService>();
            collection.AddTransient<IMinioService, MinioService>();

            // unitOfWord
            collection.AddScoped<IUnitOfWork, UnitOfWork>();

            return collection;
        }

        public static IServiceCollection AddMinioServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure MinIO settings
            services.Configure<MinioSettings>(configuration.GetSection("MinioSettings"));

            // Register MinIO client
            services.AddSingleton<IMinioClient>(serviceProvider =>
            {
                var minioSettings = serviceProvider.GetRequiredService<IOptions<MinioSettings>>().Value;
                
                return new MinioClient()
                    .WithEndpoint(minioSettings.Endpoint)
                    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
                    .WithSSL(minioSettings.UseSSL)
                    .Build();
            });

            return services;
        }




         public static IServiceCollection AddRepositories(this IServiceCollection collection)
        {
           
          // repositories
            collection.AddScoped<ITasksRepository, TasksRepository>();
            collection.AddScoped<IEmployeesRepository, EmployeesRepository>();
            collection.AddScoped<ITeamsRepository, TeamsRepository>();
            collection.AddScoped<IProjectsRepository, ProjectsRepository>();
            collection.AddScoped<ICommentsRepository, CommentsRepository>();
            collection.AddScoped<IActivitiesRepository, ActivitiesRepository>();
            collection.AddScoped<IInvitesRepository, InvitesRepository>();
            collection.AddScoped<IResetPasswordRepository, ResetPasswordRepository>();

            return collection;
        }
    }





}
