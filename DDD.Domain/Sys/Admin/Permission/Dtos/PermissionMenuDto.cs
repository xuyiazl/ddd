using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;

namespace DDD.Domain.Sys.Permission
{
    public class PermissionMenuDto : DtoKeyBase<AdminMenuEntity>
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
    }
}
