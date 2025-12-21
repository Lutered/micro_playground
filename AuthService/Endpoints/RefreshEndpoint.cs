using AuthAPI.Features.Commands.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

namespace AuthAPI.Endpoints
{
    public class RefreshEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("refresh",
                async (
                    [FromBody]string refreshToken,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new RefreshCommand(refreshToken);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.ToResult();
                }
            );
        }
    }
}
