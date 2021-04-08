using DDD.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.AddScoped(typeof(INigelDbRepository<>), typeof(NigelDbRepository<>));

            return services;
        }
    }
}
