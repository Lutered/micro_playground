using AuthAPI.Models;
using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.Register
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
