using Shared.Models.Responses.Auth;
using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Auth;

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
