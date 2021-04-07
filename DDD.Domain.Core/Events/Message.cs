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
        public string MessageType { get; protected set; }
        public long AggregateId { get; protected set; }

        protected Message()
        {
            MessageType = GetType().Name;
        }
    }
}
