using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.DTOs;
using UsersAPI.Helpers;
using UsersAPI.Infrastructure.Commands;
using UsersAPI.Infrastructure.Queries;

namespace UsersAPI.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController(
         IMediator mediator
        ) : ControllerBase
    {
        [HttpGet]
        [Route("get/{username}")]
        public async Task<ActionResult<AppUserDTO>> GetUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUserQuery(username), cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<PagedList<AppUserDTO>>> GetUsers(
            [FromQuery] PageDTO pageParams, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetUsersQuery(pageParams), cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateUser(
            AppUserDTO appUser, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new UpdateUserCommand(appUser), cancellationToken);

            if (!result.IsSuccess)
                return result.Error.Type == Shared.HandlerErrorType.NotFound ? 
                    NotFound() : 
                    BadRequest(result.Error.Message);

            return Ok(result.Value);
        }

        [HttpDelete]
        [Route("delete/{username}")]
        public async Task<ActionResult> DeleteUser(
            string username, 
            CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteUserCommand(username), cancellationToken);

            if (!result.IsSuccess)
                return result.Error.Type == Shared.HandlerErrorType.NotFound ?
                    NotFound() :
                    BadRequest(result.Error.Message);

            return Ok(result.Value);
        }
    }
}
