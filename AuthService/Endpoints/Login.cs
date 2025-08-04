using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator;
using AuthAPI.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Endpoints
{
    public class Login : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("login",
                async (
                    [FromBody] LoginDTO loginDTO,
                    IMediator mediator
                ) =>
                {
                    var result = await mediator.Send(new LoginCommand(loginDTO));

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            );
        }
    }
}
