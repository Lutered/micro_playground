using AuthAPI.Data.Entities;
using AutoMapper;
using Shared.Models.Contracts.User.PublishEvents;
using Shared.Models.Requests.Auth;
using Shared.Models.Responses.Auth;

namespace AuthAPI.Mappers
{
    public class AuthProfile : Profile
    {
        public AuthProfile() 
        {
            CreateMap<AppUser, AuthResponse>();
            CreateMap<RegisterRequest, AppUser>();
            CreateMap<AddRoleEvent, AddRoleRequest>();
            CreateMap<RemoveRoleEvent, RemoveRoleRequest>();
        }
    }
}
