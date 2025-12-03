using AuthAPI.Models;
using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.Refresh
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
