using AuthAPI.Features.Commands.RemoveRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Endpoints
{
    public class RemoveRoleEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("removeRole",
                async (
                    [FromBody] RemoveRoleRequest request,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new RemoveRoleCommand(request);
                    var result = await mediator.Send(command, cancellationToken);
                    return (IResult)result.ToActionResult();
                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
