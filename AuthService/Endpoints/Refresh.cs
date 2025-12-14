using AuthAPI.Features.Commands.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

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

                    return (IResult)result.ToActionResult();
                }
            );
        }
    }
}
