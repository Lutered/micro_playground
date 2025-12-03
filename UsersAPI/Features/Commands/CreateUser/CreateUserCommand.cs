using MediatR;
using Shared.Models.Common;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<HandlerResult<bool>>
    {
        public AppUserDTO DTO { get; set; }

        public CreateUserCommand(AppUserDTO dto) 
        {
            DTO = dto;
        }
    }
}
