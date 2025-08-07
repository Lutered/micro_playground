using AuthAPI.Infrastructure.Commands;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Endpoints
{
    public class RemoveRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("removeRole",
                async (
                    [FromBody] Shared.Contracts.Requests.User.RemoveRole removeRoleDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new RemoveRoleCommand(removeRoleDTO.Username, removeRoleDTO.RoleName), cancellationToken);

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
