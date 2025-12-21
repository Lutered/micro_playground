using AuthAPI.Features.Commands.AddRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Endpoints
{
    public class AddRoleEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("addRole",
                async (
                    [FromBody] AddRoleRequest request,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new AddRoleCommand(request);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.ToResult();

                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
