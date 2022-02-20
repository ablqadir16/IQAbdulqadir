using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Designation, o => o.MapFrom(s => s.Designation))
                .ForMember(d => d.FullAddress, o => o.MapFrom(s => s.FullAddress))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.Street))
                .ForMember(d => d.State, o => o.MapFrom(s => s.State))
                .ForMember(d => d.Pincode, o => o.MapFrom(s => s.Pincode))
                .ForMember(d => d.Country, o => o.MapFrom(s => s.Country))
                .ForMember(d => d.ImagePath, o => o.MapFrom<UserUrlResolver>());
        }
    }
}