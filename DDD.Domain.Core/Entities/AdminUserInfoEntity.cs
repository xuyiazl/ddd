using System;
using System.Collections;
using System.Collections.Generic;
using XUCore.Ddd.Domain;

namespace DDD.Domain.Core.Entities
{
    public class AdminUserInfoEntity : BaseEntity
    {
        public long UserId { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public AdminUserEntity User { get; set; }
    }
}
