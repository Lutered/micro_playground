using AuthAPI.DTOs;
using MediatR;
using Shared;

namespace AuthAPI.MediatR.Commands
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
