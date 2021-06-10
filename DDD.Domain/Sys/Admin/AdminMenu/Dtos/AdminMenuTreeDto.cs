using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuTreeDto : DtoBase<AdminMenuEntity>
    {
        public long FatherId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
        public bool IsMenu { get; set; }
        public int Weight { get; set; }
        public bool IsExpress { get; set; }

        public IList<AdminMenuTreeDto> Child { get; set; }
    }
}
