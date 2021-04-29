using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
using XUCore.NetCore.Swagger;

namespace DDD.Applaction
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("test", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"test",
                    Description = "test"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[0]
                    }
                });
                options.SwaggerHttpSignDoc(services);
                //options.SwaggerFiledDoc();

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(DependencyInjection).Assembly.Location);
                var apiXml = Path.Combine(basePath, "DDD.Applaction.xml");
                //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                options.IncludeXmlComments(apiXml);

                options.SwaggerControllerDescriptions(apiXml);

                // TODO:一定要返回true！
                options.DocInclusionPredicate((docName, description) => true);
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
        {
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger(options =>
            {
                //如果使用了 大于 5.6.3 版本，新功能Servers和反向代理的支持问题，
                //issues https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1953

                //由于使用了反向代理需要运维支持转发X-Forwarded-* headers的一些工作，所以太麻烦。故干脆清理掉算了。等官方直接解决了该问题再使用

                options.PreSerializeFilters.Add((swaggerDoc, _) =>
                {
                    swaggerDoc.Servers.Clear();
                });
                //options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                //{
                //    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host}" } };
                //});
            });
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/test/swagger.json", "test API");

                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            return app;
        }
    }
}
