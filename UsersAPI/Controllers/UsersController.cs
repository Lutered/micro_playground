using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.Features.Queries.GetUsers;
using UsersAPI.Features.Queries.GetUser;
using UsersAPI.Features.Commands.UpdateUser;
using UsersAPI.Features.Commands.DeleteUser;
using Shared.Models.Requests.User;

namespace UsersAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UsersController(
         IMediator _mediator
        ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers(
           [FromQuery] GetUsersRequest pageParams,
           CancellationToken cancellationToken = default)
        {
            var query = new GetUsersQuery(pageParams);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUser(
           string username,
           CancellationToken cancellationToken)
        {
            var query = new GetUserQuery(username);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetUserQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            Guid id,
            [FromBody] UpdateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            var command = new UpdateUserCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id), cancellationToken);

            return result.ToActionResult();
        }
    }
}
