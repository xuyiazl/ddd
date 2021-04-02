using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore;

namespace DDD.Domain.Common
{
    public static class SubCodeMessage
    {
        private static IDictionary<SubCode, (string, string)> zhCn = new Dictionary<SubCode, (string, string)> {
            { SubCode.Success,("C600000","操作成功")},
            { SubCode.Fail,("C600001","操作失败")},
            { SubCode.GlobalError,("C600002","操作异常")},
            { SubCode.Unauthorized,("C600003","操作无权限")},
            { SubCode.Cancel,("C600004","操作取消")},
            { SubCode.Repetition,("C600005","数据重复")},
            { SubCode.SoldOut,("C600006","数据已下架")},
            { SubCode.Undefind,("C600007","数据不存在")},
            { SubCode.BadWord,("C600008","您发表的内容包含违禁词！")},
            { SubCode.TaskCanceled,("C600010","服务器执行任务超时，任务被取消")}
        };

        public static (string, string) Message(SubCode subCode)
        {
            return zhCn[subCode];
        }
    }
    public enum SubCode
    {
        Success,
        Fail,
        GlobalError,
        Unauthorized,
        Cancel,
        TaskCanceled,
        Repetition,
        SoldOut,
        Undefind,
        BadWord
    }


    public static class ResultModel
    {
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Success<T>(string subCode, string message, T data = default) =>
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
        public static Result<T> Success<T>(SubCode subCode, T data = default)
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
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Fail<T>(string subCode, string message, T data = default) =>
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
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Fail<T>(SubCode subCode, T data = default)
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
