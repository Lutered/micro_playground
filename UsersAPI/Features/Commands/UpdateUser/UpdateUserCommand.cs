using MediatR;
using Shared.Models.Common;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Commands.UpdateUser
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
