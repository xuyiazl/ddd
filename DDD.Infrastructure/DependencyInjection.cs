using DDD.Domain.Common.Mappings;
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
using XUCore.NetCore.Jwt;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Redis;
using XUCore.Serializer;

namespace DDD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, string project = "api")
        {
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

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

            // 注入redis插件，支持拦截器缓存
            services.AddRedisService().AddJsonRedisSerializer();
            // 注入缓存拦截器（缓存数据用）
            services.AddCacheService<RedisCacheService>((option) =>
            {
                option.RedisRead = "cache-read";
                option.RedisWrite = "cache-write";
            });

            // 注入jwt（XUCore.NetCore.Jwt）
            var jwtSettings = services.BindSection<JwtOptions>(configuration, "JwtOptions");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwt(JwtAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Keys = new[] { jwtSettings.Secret };
                options.VerifySignature = true;
            });


            // 以下部分为微软官方提供的JWT
            // 注入jwt（Microsoft.AspNetCore.Authentication.JwtBearer）
            //services.AddAuthentication(options=> {
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //   .AddJwtBearer(o =>
            //   {
            //       o.TokenValidationParameters = new TokenValidationParameters()
            //       {
            //           ValidateIssuerSigningKey = true,
            //           ValidIssuer = "",
            //           ValidAudience = "",
            //           //用于签名验证
            //           IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
            //           ValidateIssuer = false,
            //           ValidateAudience = false,
            //           ValidateActor = false
            //       };
            //   });


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

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env, string project = "api")
        {
            if (project == "api")
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseStaticFiles();
            }
            else
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                }
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //全局验证jwt
            //app.UseJwtMiddleware();

            app.UseStaticHttpContext();

            if (project == "api")
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                });
            }

            return app;
        }
    }
}
