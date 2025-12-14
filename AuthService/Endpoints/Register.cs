using AuthAPI.Features.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;
using Shared.Models.DTOs.Auth;

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
