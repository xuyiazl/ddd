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

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<AdminUserEntity, AdminUserDto>();
            //.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            //.ForMember(d => d.Mobile, opt => opt.MapFrom(s => s.Mobile))
            //.ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
            //.ForMember(d => d.Picture, opt => opt.MapFrom(s => s.Picture))
            //.ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location))
            //.ForMember(d => d.Position, opt => opt.MapFrom(s => s.Position))
            //.ForMember(d => d.Company, opt => opt.MapFrom(s => s.Company));
        }
    }
}
