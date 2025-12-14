using Shared.Models.Responses.Auth;
using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Auth;

namespace AuthAPI.Features.Commands.Login
{
    public class LoginCommand : IRequest<HandlerResult<AuthResponseDTO>>
    {
        public LoginDTO DTO { get; private set; }

        public LoginCommand(LoginDTO dto)
        {
            DTO = dto;
        }
    }
}
