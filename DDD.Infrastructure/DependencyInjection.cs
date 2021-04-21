﻿using DDD.Domain.Common.Mappings;
using DDD.Infrastructure.Bus;
using DDD.Infrastructure.Events;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using XUCore.Ddd.Domain;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.MessagePack;
using XUCore.Serializer;

namespace DDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, string project = "api")
        {
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(IMapFrom<>));
            services.AddMediatR(typeof(IMapFrom<>));

            services.AddPerformanceBehaviour();

            services.AddValidationBehavior();

            // 命令总线Domain Bus (Mediator)
            services.AddMediatorBus<InMemoryBus>();

            // 注入 基础设施层 - 事件溯源
            //services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddEventStore<SqlEventStoreService>();
            //services.AddScoped<EventStoreSQLContext>();

            //services.AddScoped<IUserManager, UserManagerService>();

            //services.AddAuthentication();

            IMvcBuilder mvcBuilder;

            if (project.Equals("api"))
            {
                mvcBuilder = services.AddControllers();

                services.AddDynamicWebApi();
            }
            else
            {
                mvcBuilder = services.AddControllersWithViews();
            }

            mvcBuilder
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


            //services.AddCacheService<MemoryCacheService>();

            return services;
        }
    }
}
