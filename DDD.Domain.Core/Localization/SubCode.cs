using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore;

namespace DDD.Domain.Core
{
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
