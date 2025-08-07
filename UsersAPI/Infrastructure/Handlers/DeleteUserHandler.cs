using AutoMapper;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using Shared.Contracts;
using UsersAPI.Data.Entities;
using UsersAPI.Extensions;
using UsersAPI.Infrastructure.Cache;
using UsersAPI.Infrastructure.Commands;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Infrastructure.Handlers
{
    public class DeleteUserHandler(
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
                    new HandlerError($"User {username} does not exists", HandlerErrorType.NotFound));

            _userRepository.RemoveUser(user);
            if(!await _userRepository.SaveChangesAsync(cancellationToken))
            {
                return HandlerResult<bool>.Failure(
                   new HandlerError($"Something wend wrong during deleting"));
            }

            await _publishEndpoint.Publish<UserDeleted>(new UserDeleted
            {
                Username = username
            });

            await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");
            await _cache.RemoveAsync($"{CONSTS.USER_PREFIX}:user:{username}");

            _logger.LogInformation($"User {user.Username} was deleted");

            return HandlerResult<bool>.Success(true);
        }
    }
}
