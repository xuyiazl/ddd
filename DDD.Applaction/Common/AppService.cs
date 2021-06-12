using DDD.Applaction.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Infrastructure.Filters;
using Microsoft.AspNetCore.Authorization;
using System;
using XUCore.Ddd.Domain.Bus;
using XUCore.NetCore;
using XUCore.NetCore.DynamicWebApi;

namespace DDD.Applaction.Common
{
    //[DynamicWebApi(Module = "v1")]
    [DynamicWebApi]
    [ApiError]
    [ApiElapsedTime]
    [Authorize]
    public class AppService : IAppService
    {
        // 中介者 总线
        public readonly IMediatorHandler bus;

        public AppService(IMediatorHandler bus)
        {
            this.bus ??= bus;
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(string subCode, string message, T data = default) =>
            new Result<T>()
            {
                code = 0,
                subCode = subCode,
                message = message,
                data = data,
                elapsedTime = -1,
                operationTime = DateTime.Now
            };
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(SubCode subCode, T data = default)
        {
            (var code, var message) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                code = 0,
                subCode = code,
                message = message,
                data = data,
                elapsedTime = -1,
                operationTime = DateTime.Now
            };
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(SubCode subCode, string message, T data = default)
        {
            (var code, _) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                code = 0,
                subCode = code,
                message = message,
                data = data,
                elapsedTime = -1,
                operationTime = DateTime.Now
            };
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(string subCode, string message, T data = default) =>
             new Result<T>()
             {
                 code = 0,
                 subCode = subCode,
                 message = message,
                 data = data,
                 elapsedTime = -1,
                 operationTime = DateTime.Now
             };
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(SubCode subCode, string message, T data = default)
        {
            (var code, _) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                code = 0,
                subCode = code,
                message = message,
                data = data,
                elapsedTime = -1,
                operationTime = DateTime.Now
            };
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(SubCode subCode, T data = default)
        {
            (var code, var message) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                code = 0,
                subCode = code,
                message = message,
                data = data,
                elapsedTime = -1,
                operationTime = DateTime.Now
            };
        }
    }
}
