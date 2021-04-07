using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore;

namespace DDD.Domain.Core
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
}
