using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleDto : DtoBase<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
    }
}
