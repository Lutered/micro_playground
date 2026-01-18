using AuthAPI.Features.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces.Common;
using Shared.Models.Requests.Auth;

namespace AuthAPI.Endpoints
{
    public class RegisterEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("register", 
                async (
                    [FromBody]RegisterRequest request,
                    IMediator mediator,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new RegisterCommand(request);
                    var result = await mediator.Send(command, cancellationToken);
                    return result.ToResult();
                }
            );
        }
    }
}
