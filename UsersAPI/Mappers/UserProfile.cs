using AutoMapper;
using Shared.Models.Contracts.User.PublishEvents;
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
            CreateMap<UserCreatedEvent, UserDTO>();
        }
    }
}
