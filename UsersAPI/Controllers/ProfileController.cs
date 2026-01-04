using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Requests.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsersAPI.Features.Commands.UpdateUser;
using UsersAPI.Features.Queries.GetUser;
using UsersAPI.Features.Queries.GetUsers;

namespace UsersAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProfileController(
        IMediator _mediator
       ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken = default)
        {
            string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return BadRequest("User Id was not found");

            var result = await _mediator.Send(new GetUserQuery(userId), cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            string userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return BadRequest("User Id was not found");

            var command = new UpdateUserCommand(userId, request);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }
    }
}
