using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuDto : DtoBase<AdminMenuEntity>
    {
        public long FatherId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
        public bool IsMenu { get; set; }
        public int Weight { get; set; }
        public bool IsExpress { get; set; }
    }
}
