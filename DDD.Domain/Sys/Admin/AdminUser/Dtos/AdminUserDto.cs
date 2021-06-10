using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserDto : DtoBase<AdminUserEntity>
    {
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int LoginCount { get; set; }
        public DateTime LoginLastTime { get; set; }
        public string LoginLastIp { get; set; }
    }
}
