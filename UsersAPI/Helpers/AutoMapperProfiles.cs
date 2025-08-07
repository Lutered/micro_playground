using AutoMapper;
using Shared.Contracts;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;

namespace UsersAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUserDTO, User>();
            CreateMap<User, AppUserDTO>();
            CreateMap<UserCreated, AppUserDTO>();
        }
    }
}
