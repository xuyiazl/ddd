using DDD.Domain.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NigelDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString: configuration.GetConnectionString("NigelDBConnection"),
                    sqlServerOptionsAction: options =>
                    {
                        options.EnableRetryOnFailure();
                        //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
                        //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
                        options.MigrationsAssembly("DDD.Persistence");
                    }
                    )
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                //options.UseLoggerFactory(MyLoggerFactory);
            });

            // 5.0.0 mysql 暂时支持的只有预览版

            //services.AddDbContext<NigelDbContext>(options =>
            //{
            //    options.UseMySql(
            //        connectionString: config.GetConnectionString("NigelDB_Connection"),
            //        serverVersion: new MySqlServerVersion(new Version(8, 0, 21)),
            //        mySqlOptionsAction: options =>
            //        {
            //            options.CharSetBehavior(CharSetBehavior.NeverAppend);
            //            options.EnableRetryOnFailure();
            //            //options.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c.CurrentContext.Context));
            //            //options.ExecutionStrategy(c => new SqlServerRetryingExecutionStrategy(c.CurrentContext.Context));
            //        }
            //        )
            //    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            //    //options.UseLoggerFactory(MyLoggerFactory);
            //});

            services.AddScoped(typeof(INigelDbContext), typeof(NigelDbContext));
            services.AddScoped(typeof(INigelDbRepository), typeof(NigelDbRepository));

            return services;
        }

        public static IApplicationBuilder UsePersistence(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var dbContext = scope.ServiceProvider.GetService<NigelDbContext>();

            dbContext.Database.Migrate();

            return app;
        }
    }
}
