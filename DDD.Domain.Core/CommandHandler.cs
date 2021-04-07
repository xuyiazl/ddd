using DDD.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Core
{
    /// <summary>
    /// 领域命令处理程序
    /// 用来作为全部处理程序的基类，提供公共方法和接口数据
    /// </summary>
    public class CommandHandler
    {
        // 注入中介处理接口（目前用不到，在领域事件中用来发布事件）
        public readonly IMediatorHandler bus;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="bus"></param>
        public CommandHandler(IMediatorHandler bus)
        {
            this.bus = bus;
        }
    }
}
