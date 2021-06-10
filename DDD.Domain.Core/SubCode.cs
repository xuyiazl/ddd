using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Core
{
    public static class SubCodeMessage
    {
        private static IDictionary<SubCode, (string, string)> zhCn = new Dictionary<SubCode, (string, string)> {
            { SubCode.Success,("C000000","操作成功")},
            { SubCode.Fail,("C000001","操作失败")},
            { SubCode.GlobalError,("C000002","操作异常")},
            { SubCode.Unauthorized,("C000003","操作无权限")},
            { SubCode.Cancel,("C000004","操作取消")},
            { SubCode.Repetition,("C000005","数据重复")},
            { SubCode.SoldOut,("C000006","数据已下架")},
            { SubCode.Undefind,("C000007","数据不存在")},
            { SubCode.BadWord,("C000008","您发表的内容包含违禁词！")},
            { SubCode.RepetitionSubmit,("C000009","重复提交")},
            { SubCode.TaskCanceled,("C000010","服务器执行任务超时，任务被取消")},
            { SubCode.Max,("C000011","超过限制")},
            { SubCode.MaxTime,("C000012","时间已到，无法操作")},
            { SubCode.SignFail,("C000013","签名失败")},
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
        BadWord,
        RepetitionSubmit,
        Max,
        MaxTime,
        SignFail
    }
}
