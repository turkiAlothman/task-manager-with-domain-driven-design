using Application.Services.Interfaces;
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
            collection.AddTransient<IEmailService, EmailService>();
            collection.AddTransient<ITasksService, TasksService>();

            return collection;
        }
    }
}
