using AuthAPI.Data.Entities;
using AutoMapper;
using Contracts.AuthApi.Requests;

namespace AuthAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<AppUser, Register>();
            CreateMap<AppUser, Login>();
        }
    }
}
