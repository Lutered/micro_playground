using AutoMapper;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using Shared.Models.Contracts.Requests.User;
using UsersAPI.Data.Entities;
using UsersAPI.Extensions;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
        IUserRepository _userRepository,
        IPublishEndpoint _publishEndpoint,
        IDistributedCache _cache,
        ILogger _logger
    )
    : IRequestHandler<DeleteUserCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var username = request.Username;
            var user = await _userRepository.GetUserAsync(username, cancellationToken);

            if (!await _userRepository.UserExists(username))
                return HandlerResult<bool>.Failure(
                    HandlerErrorType.NotFound, 
                    $"User {username} does not exists");

            _userRepository.RemoveUser(user);
            if(!await _userRepository.SaveChangesAsync(cancellationToken))
            {
                return HandlerResult<bool>.Failure(
                     HandlerErrorType.BadRequest,
                     "Something wend wrong during deleting");
            }

            await _publishEndpoint.Publish(new UserDeleted
            {
                Username = username
            });

           // await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");
           // await _cache.RemoveAsync($"{CONSTS.USER_PREFIX}:user:{username}");

            _logger.LogInformation($"User {user.Username} was deleted");

            return HandlerResult<bool>.Success(true);
        }
    }
}
