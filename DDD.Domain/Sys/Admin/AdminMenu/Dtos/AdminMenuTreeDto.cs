﻿using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuTreeDto : DtoBase<AdminMenuEntity>
    {
        /// <summary>
        /// 导航父级id
        /// </summary>
        public long FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }

        public IList<AdminMenuTreeDto> Child { get; set; }
    }
}
