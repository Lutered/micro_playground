using AutoMapper;
using Shared.Models.Contracts.User.Events;
using Shared.Models.Contracts.User.Requests.CreateUser;
using Shared.Models.DTOs.User;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;

namespace UsersAPI.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<CreateUserRequest, UserDTO>();
        }
    }
}
