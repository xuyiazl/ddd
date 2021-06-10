using DDD.Applaction.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Applaction
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, string project = "api")
        {
            services.Scan(scan =>
                scan.FromAssemblyOf<IAppService>()
                .AddClasses(impl => impl.AssignableTo(typeof(IAppService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

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
