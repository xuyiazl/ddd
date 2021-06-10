﻿using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Sys.Permission
{
    public class PermissionViewModel
    {
        public IList<AdminMenuEntity> Menus { get; set; }
        public IList<AdminRoleMenuEntity> RoleMenus { get; set; }
        public IList<AdminUserRoleEntity> UserRoles { get; set; }
    }
}
