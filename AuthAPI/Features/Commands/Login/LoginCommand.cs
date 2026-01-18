using Shared.Models.Responses.Auth;
using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Features.Commands.Login
{
    public record class LoginCommand : IRequest<HandlerResult<AuthResponse>>
    {
        public LoginRequest Input { get; init; }

        public LoginCommand(LoginRequest input)
        {
            Input = input;
        }
    }
}
