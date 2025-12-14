using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.DTOs;
using UsersAPI.Features.Queries.GetUsers;
using UsersAPI.Features.Queries.GetUser;
using UsersAPI.Features.Commands.UpdateUser;
using UsersAPI.Features.Commands.DeleteUser;
using System.IdentityModel.Tokens.Jwt;
using Shared.Models.Requests;
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
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken) 
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
           [FromQuery] PagedRequest pageParams,
           CancellationToken cancellationToken)
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
            var command = new UpdateUserCommand(updateRequest);
            var result = await mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteUserCommand(username), cancellationToken);

            return result.ToActionResult();
        }
    }
}
