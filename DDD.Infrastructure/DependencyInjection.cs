using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Infrastructure.Bus;
using DDD.Infrastructure.Events;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using XUCore.Configs;
using XUCore.Ddd.Domain;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Redis;
using XUCore.Serializer;

namespace DDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(IMapFrom<>));
            services.AddMediatR(typeof(IMapFrom<>));

            services.AddRequestBehaviour(options =>
            {
                options.Logger = true;
                options.Performance = true;
                options.Validation = true;
            });

            // 命令总线Domain Bus (Mediator)
            services.AddMediatorBus<InMemoryBus>();

            // 注入 基础设施层 - 事件溯源
            //services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddEventStore<SqlEventStoreService>();
            //services.AddScoped<EventStoreSQLContext>();

            //services.AddScoped<IUserManager, UserManagerService>();

            //services.AddAuthentication();

            // 注入redis插件，支持拦截器缓存
            services.AddRedisService().AddJsonRedisSerializer();
            // 注入缓存拦截器（缓存数据用）
            services.AddCacheService<RedisCacheService>((option) =>
            {
                option.RedisRead = "cache-read";
                option.RedisWrite = "cache-write";
            });

            // 注入jwt（XUCore.NetCore.Jwt）
            var jwtSettings = services.BindSection<JwtSettings>(configuration, "JwtSettings");

            // 注入jwt（Microsoft.AspNetCore.Authentication.JwtBearer）
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    //用于签名验证
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateActor = false
                };
            });

            services
               .AddControllers()
               .AddMessagePackFormatters(options =>
               {
                   options.JsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                   options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver();

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

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticHttpContext();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
