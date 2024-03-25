using Application.Services.Interfaces;
using Domain.Base;
using infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
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


            // unitOfWord
            collection.AddScoped<IUnitOfWork, UnitOfWork>();



            //repositories
            //collection.AddScoped


            return collection;
        }
    }
}
