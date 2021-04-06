using AutoMapper;
using DDD.Domain.Entities;
using DDD.Domain.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers.Dtos
{
    public class AdminUserDto : IMapFrom<AdminUser>
    {
        public long Id { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AdminUser, AdminUserDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Mobile, opt => opt.MapFrom(s => s.Mobile))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.Picture, opt => opt.MapFrom(s => s.Picture))
                .ForMember(d => d.Location, opt => opt.MapFrom(s => s.Location))
                .ForMember(d => d.Position, opt => opt.MapFrom(s => s.Position))
                .ForMember(d => d.Company, opt => opt.MapFrom(s => s.Company));
        }
    }
}
