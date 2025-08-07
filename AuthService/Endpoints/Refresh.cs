using AuthAPI.Intrefaces;
using AuthAPI.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Endpoints
{
    public class Refresh : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("refresh",
                async (
                    [FromBody] string refreshToken,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new RefreshCommand(refreshToken), cancellationToken);

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            );
        }
    }
}
