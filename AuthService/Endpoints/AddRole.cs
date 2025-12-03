using AuthAPI.Features.Commands.AddRole;
using AuthAPI.Intrefaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;

namespace AuthAPI.Endpoints
{
    public class AddRole : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("addRole",
                async (
                    [FromBody] Shared.Models.Contracts.Requests.User.AddRole addRoleDTO,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var result = await mediator.Send(new AddRoleCommand(addRoleDTO.Username, addRoleDTO.RoleName), cancellationToken);

                    return (IResult)result.ToActionResult();

                }
            ).RequireAuthorization(policy => policy.RequireRole("Admin"));
        }
    }
}
