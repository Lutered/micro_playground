using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AutoMapper;

namespace AuthAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<AppUser, AuthResponseDTO>();
            CreateMap<RegisterDTO, AppUser>();
        }
    }
}
