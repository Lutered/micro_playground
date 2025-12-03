using AuthAPI.Features.Commands.RemoveRole;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

namespace AuthAPI.Endpoints
{
    public class RemoveRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("removeRole",
                async (
                    [FromBody] Shared.Models.Contracts.Requests.User.RemoveRole removeRoleDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new RemoveRoleCommand(removeRoleDTO.Username, removeRoleDTO.RoleName), cancellationToken);

                    return (IResult)result.ToActionResult();
                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
