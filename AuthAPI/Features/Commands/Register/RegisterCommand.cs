using Shared.Models.Responses.Auth;
using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Features.Commands.Register
{
    public record class RegisterCommand : IRequest<HandlerResult<AuthResponse>>
    {
        public RegisterRequest Input { get; init; }

        public RegisterCommand(RegisterRequest input) 
        {
            Input = input; 
        }
    }
}
