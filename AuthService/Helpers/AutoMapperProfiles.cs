using AuthAPI.Data.Entities;
using Shared.Models.Responses.Auth;
using AuthAPI.Features.Commands.Register;
using AutoMapper;
using Shared.Models.DTOs.Auth;

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
