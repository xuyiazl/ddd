using AutoMapper;
using DDD.Domain.Common.Mappings;
using System;

namespace DDD.Domain.Sys.LoginRecord
{
    public class LoginRecordDto : DtoBase<LoginRecordViewModel>
    {
        public long AdminId { get; set; }
        public string LoginWay { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginIp { get; set; }

        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}
