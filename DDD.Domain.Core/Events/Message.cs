using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Core.Events
{
    /// <summary>
    /// 抽象类Message，用来获取我们事件执行过程中的类名
    /// 然后并且添加聚合根
    /// </summary>
    public abstract class Message<TResponse> : IRequest<TResponse>
    {
        /// <summary>
        /// 消息类型（执行操作的命令）
        /// </summary>
        public string MessageType { get; protected set; }
        /// <summary>
        /// 聚合根（表主键）
        /// </summary>
        public long AggregateId { get; protected set; }
        /// <summary>
        /// 聚合类型（实体名，如果聚合根使用Guid可以不使用，避免查询的时候主键冲突）
        /// </summary>
        public string AggregateType { get;protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
