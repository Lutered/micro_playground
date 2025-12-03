using AuthAPI.Features.Commands.Login;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

namespace AuthAPI.Endpoints
{
    public class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("login",
                async (
                    [FromBody] LoginDTO loginDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new LoginCommand(loginDTO), cancellationToken);

                    return (IResult)result.ToActionResult();
                }
            );
        }
    }
}
