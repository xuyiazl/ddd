using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core.Entities;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserDto : DtoBase<AdminUserEntity>
    {
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }

        public override void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserEntity, AdminUserDto>();
    }
}
