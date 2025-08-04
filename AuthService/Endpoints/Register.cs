using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Endpoints
{
    public class Register : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("register", 
                async (
                    [FromBody]RegisterDTO registerDTO,
                    IMediator mediator
                ) =>
                {
                    var result = await mediator.Send(new RegisterCommand(registerDTO));

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            );
        }
    }
}
