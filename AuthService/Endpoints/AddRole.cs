using AuthAPI.Infrastructure.Commands;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Endpoints
{
    public class AddRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("addRole",
                async (
                    [FromBody] Shared.Contracts.Requests.User.AddRole addRoleDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new AddRoleCommand(addRoleDTO.Username, addRoleDTO.RoleName), cancellationToken);

                    if (!result.IsSuccess)
                        return Results.BadRequest(result.Error.Message);

                    return Results.Ok(result.Value);
                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
