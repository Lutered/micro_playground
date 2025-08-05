using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Infrastructure.Endpoints
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

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            );
        }
    }
}
