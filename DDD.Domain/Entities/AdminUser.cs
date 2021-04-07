﻿using DDD.Domain.Common;
using System;

namespace DDD.Domain.Entities
{
    public class AdminUser : Entity, IAggregateRoot
    {
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int LoginCount { get; set; }
        public DateTime LoginLastTime { get; set; }
        public string LoginLastIp { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Status { get; set; }
    }
}
