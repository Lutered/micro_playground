using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.DTOs;
using UsersAPI.Features.Queries.GetUsers;
using UsersAPI.Features.Queries.GetUser;
using UsersAPI.Features.Commands.UpdateUser;
using UsersAPI.Features.Commands.DeleteUser;

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
        [HttpGet]
        public async Task<IActionResult> GetUsers(
           [FromQuery] PageDTO pageParams,
           CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUsersQuery(pageParams), cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserQuery(username), cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(
            AppUserDTO appUser, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UpdateUserCommand(appUser), cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteUserCommand(username), cancellationToken);

            return result.ToActionResult();
        }
    }
}
