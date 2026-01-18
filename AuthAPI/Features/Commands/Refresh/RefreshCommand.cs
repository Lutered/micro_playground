using Shared.Models.Responses.Auth;
using MediatR;
using Shared.Models.Common;

namespace AuthAPI.Features.Commands.Refresh
{
    public class RefreshCommand : IRequest<HandlerResult<AuthResponse>>
    {
        public string Token { get; private set; }

        public RefreshCommand(string token)
        {
            Token = token;
        }
    }
}
