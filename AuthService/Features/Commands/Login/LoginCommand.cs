using AuthAPI.Models;
using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.Login
{
    public class LoginCommand : IRequest<HandlerResult<AuthResponseDTO>>
    {
        public LoginDTO DTO { get; set; }

        public LoginCommand(LoginDTO dto)
        {
            DTO = dto;
        }
    }
}
