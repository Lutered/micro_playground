using AuthAPI.Data.Entities;
using AuthAPI.Models;
using AuthAPI.Features.Commands.Register;
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
