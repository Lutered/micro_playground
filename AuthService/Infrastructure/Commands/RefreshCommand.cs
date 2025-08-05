using AuthAPI.DTOs;
using MediatR;
using Shared;

namespace AuthAPI.MediatR.Commands
{
    public class RefreshCommand : IRequest<HandlerResult<AuthResponseDTO>>
    {
        public string Token { get; set; }

        public RefreshCommand(string token)
        {
            Token = token;
        }
    }
}
