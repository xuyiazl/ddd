using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.AdminUsers.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DDD.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAdminUserAppService, AdminUserAppService>();

            return services;
        }
    }
}
