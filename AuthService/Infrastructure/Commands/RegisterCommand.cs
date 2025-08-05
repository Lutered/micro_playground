using AuthAPI.DTOs;
using MediatR;
using Shared;

namespace AuthAPI.Mediator.Commands
{
    public class RegisterCommand : IRequest<HandlerResult<AuthResponseDTO>>
    {
        public RegisterDTO DTO { get; set; }

        public RegisterCommand(RegisterDTO dto) 
        {
            DTO = dto; 
        }
    }
}
