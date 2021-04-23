using System;
using XUCore.Ddd.Domain;

namespace DDD.Domain.Core.Entities
{
    public class AdminUserLoginRecordEntity : Entity, IAggregateRoot
    {
        public long UserId { get; set; }
        public string Mode { get; set; }
        public DateTime LoginTime { get; set; }
        public AdminUserEntity User { get; set; }
    }
}
