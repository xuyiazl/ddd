using DDD.Domain;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Events;
using DDD.Domain.Core.Interfaces;
using DDD.Infrastructure.Behaviours;
using DDD.Infrastructure.Bus;
using DDD.Infrastructure.Events;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.MessagePack;
using XUCore.Serializer;

namespace DDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(IMapFrom<>));
            services.AddMediatR(typeof(IMapFrom<>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            // 命令总线Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // 注入 基础设施层 - 事件溯源
            //services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddScoped<IEventStoreService, SqlEventStoreService>();
            //services.AddScoped<EventStoreSQLContext>();

            //services.AddScoped<IUserManager, UserManagerService>();

            //services.AddAuthentication();

            services.AddControllers()
                .AddMessagePackFormatters(options =>
                {
                    options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.JsonSerializerSettings.ContractResolver = new LimitPropsCamelCaseContractResolver();

                    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
                    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
                    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
                    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;

                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining(typeof(IMapFrom<>)));


            services.AddDynamicWebApi();

            //services.AddCacheService<MemoryCacheService>();

            return services;
        }
    }
}
