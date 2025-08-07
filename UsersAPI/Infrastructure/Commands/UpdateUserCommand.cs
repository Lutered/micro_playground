using MediatR;
using Shared;
using UsersAPI.DTOs;

namespace UsersAPI.Infrastructure.Commands
{
    public class UpdateUserCommand : IRequest<HandlerResult<bool>>
    {
        public AppUserDTO DTO { get; set; }

        public UpdateUserCommand(AppUserDTO dto)
        {
            DTO = dto;
        }
    }
}
