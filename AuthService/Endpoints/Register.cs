using AuthAPI.Features.Commands.Register;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

namespace AuthAPI.Endpoints
{
    public class Register : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("register", 
                async (
                    [FromBody]RegisterDTO registerDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new RegisterCommand(registerDTO), cancellationToken);

                    return (IResult)result.ToActionResult();
                }
            );
        }
    }
}
