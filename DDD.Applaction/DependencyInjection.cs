using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.AdminUsers.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DDD.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, string project = "api")
        {
            services.AddScoped<IAdminUserAppService, AdminUserAppService>();

            if (project == "api")
            {
                services.AddSwagger();
            }

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, string project = "api")
        {
            if (project == "api")
            {
                app.UseSwagger();
            }

            return app;
        }
    }
}
