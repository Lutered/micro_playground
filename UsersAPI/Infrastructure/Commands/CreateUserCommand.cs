using MediatR;
using Shared;
using UsersAPI.DTOs;

namespace UsersAPI.Infrastructure.Commands
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
