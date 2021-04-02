using DDD.Domain.Common.Interfaces;
using DDD.Infrastructure.Behaviours;
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
            services.AddMediatR(Assembly.GetAssembly(typeof(INigelDbContext)));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(RequestLogger<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            //services.AddScoped<IUserManager, UserManagerService>();
            services.AddScoped<INotificationService, NotificationService>();

            //services.AddAuthentication();

            services.AddHttpContextAccessor();

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
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<INigelDbContext>());


            services.AddDynamicWebApi();

            return services;
        }
    }
}
