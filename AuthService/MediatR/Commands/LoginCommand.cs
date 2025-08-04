using AuthAPI.DTOs;
using AuthAPI.Mediator;
using MediatR;

namespace AuthAPI.MediatR.Commands
{
    public class LoginCommand : IRequest<Result<AuthResponseDTO>>
    {
        public LoginDTO DTO { get; set; }

        public LoginCommand(LoginDTO dto)
        {
            DTO = dto;
        }
    }
}
