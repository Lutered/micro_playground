using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Features.Queries.GetUsers;
using UsersAPI.Features.Queries.GetUser;
using UsersAPI.Features.Commands.UpdateUser;
using UsersAPI.Features.Commands.DeleteUser;
using System.IdentityModel.Tokens.Jwt;
using Shared.Models.Requests.User;

namespace UsersAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("{controller}")]
    [Produces("application/json")]
    public class UsersController(
         IMediator mediator
        ) : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken = default) 
        {
            string userIdStr = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (!Guid.TryParse(userIdStr, out Guid userId)) 
                return BadRequest("User Id was not found");

            var result = await mediator.Send(new GetUserQuery(userId), cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers(
           [FromQuery] GetUsersRequest pageParams,
           CancellationToken cancellationToken = default)
        {
            var query = new GetUsersQuery(pageParams);
            var result = await mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var query = new GetUserQuery(username);
            var result = await mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(
            Guid id,
            UpdateUserRequest updateRequest, 
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateUserCommand(id, updateRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(
            Guid id, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteUserCommand(id), cancellationToken);

            return result.ToActionResult();
        }
    }
}
