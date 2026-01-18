using AuthAPI.Features.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Endpoints
{
    public class LoginEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("login",
                async (
                    [FromBody] LoginRequest request,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new LoginCommand(request);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.ToResult();
                }
            );
        }
    }
}
