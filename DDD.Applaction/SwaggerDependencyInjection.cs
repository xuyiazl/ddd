using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using System.Linq;
using System.Reflection;
using XUCore.Extensions;
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
                options.SwaggerDoc("test1", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"test",
                    Description = "test"
                });

                options.AddJwtBearerDoc();
                options.AddHttpSignDoc(services);
                //options.AddFiledDoc();

                options.AddDescriptions(typeof(DependencyInjection), "DDD.Applaction.xml", "DDD.Domain.xml", "DDD.Domain.Core.xml");

                // TODO:一定要返回true！true 分组无效 注释掉 必须有分组才能出现api
                //options.DocInclusionPredicate((docName, description) => true);
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
                c.AddMiniProfiler();

                c.SwaggerEndpoint($"/swagger/test/swagger.json", "test API");
                c.SwaggerEndpoint($"/swagger/test1/swagger.json", "test1 API");

                c.DocExpansion(DocExpansion.None);
            });

            return app;
        }
    }
}
