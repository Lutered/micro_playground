using AuthAPI.DTOs;
using MediatR;

namespace AuthAPI.Mediator.Commands
{
    public class RegisterCommand : IRequest<Result<AuthResponseDTO>>
    {
        public RegisterDTO DTO { get; set; }

        public RegisterCommand(RegisterDTO dto) 
        {
            DTO = dto; 
        }
    }
}
