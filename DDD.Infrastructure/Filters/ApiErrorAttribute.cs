using DDD.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Extensions;
using XUCore.NetCore.Properties;

namespace DDD.Infrastructure.Filters
{

    /// <summary>
    /// API错误日志过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiErrorAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context">异常上下文</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
                return;

            if (context.Exception is OperationCanceledException)
            {
                context.ExceptionHandled = true;
                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, R.CanceledMessage))
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, context.Exception.Message))
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
            }
            else if (context.Exception is ValidationException)
            {
                var ex = context.Exception as ValidationException;

                var message = ex.Failures.Select(c => c.Value.Join("，")).Join("");

                var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<SubCode>>();

                var local = localizer[SubCode.Fail.GetName()];

                context.Result = new ObjectResult(new Result<object>()
                {
                    code = 0,
                    subCode = local.Value,
                    message = message,
                    data = null,
                    elapsedTime = -1,
                    operationTime = DateTime.Now
                })
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
            else
            {
                var logger = context.HttpContext.RequestServices.GetService<ILogger<ApiErrorAttribute>>();

                if (logger.IsEnabled(LogLevel.Error))
                {
                    var routes = context.GetRouteValues().Select(c => $"{c.Key}={c.Value}").Join("，");

                    var str = new Str();
                    str.AppendLine("WebApi全局异常");
                    str.AppendLine($"路由信息：{routes}");
                    str.AppendLine($"IP：{context.HttpContext.Connection.RemoteIpAddress}");
                    str.AppendLine($"请求方法：{context.HttpContext.Request.Method}");
                    str.AppendLine($"请求地址：{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}");
                    logger.LogError(context.Exception.FormatMessage(str.ToString()));
                }

                context.Result = new ObjectResult(new Result<object>(StateCode.Fail, context.Exception.Message))
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };

                base.OnException(context);
            }
        }
    }
}
