using System;
using System.Collections.Generic;
using XUCore.Ddd.Domain;

namespace DDD.Domain.Core.Entities
{
    public class AdminUserEntity : BaseEntity
    {
        public AdminUserEntity()
        {
            LoginRecords = new HashSet<AdminUserLoginRecordEntity>();
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public AdminUserInfoEntity UserInfo { get; set; }
        public ICollection<AdminUserLoginRecordEntity> LoginRecords { get; set; }
    }
}
