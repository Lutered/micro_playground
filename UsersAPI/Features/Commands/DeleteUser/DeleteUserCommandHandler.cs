using AutoMapper;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.Contracts.User.PublishEvents;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.Extensions;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
        IUserRepository _userRepository,
        IPublishEndpoint _publishEndpoint,
        IDistributedCache _cache,
        ILogger<DeleteUserCommandHandler> _logger,
        UserCacheHelper _userCacheHelper
    )
    : IRequestHandler<DeleteUserCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            var user = await _userRepository.GetUserAsync(userId, cancellationToken);

            if (user == null)
                return HandlerResult.Failure(
                    HandlerErrorType.NotFound, 
                    $"User with Id {userId} does not exists");

            _userRepository.RemoveUser(user);
            if(!await _userRepository.SaveChangesAsync(cancellationToken))
            {
                return HandlerResult.Failure(
                     HandlerErrorType.BadRequest,
                     "Something wend wrong during deleting");
            }

            await _publishEndpoint.Publish(new UserDeletedEvent
            {
                Id = userId
            });

            await _cache.UpdateVersionAsync(CacheKeys.User.List);
            await _userCacheHelper.ClearUserCache(user);

            _logger.LogInformation($"User {user.Username} was deleted");

            return HandlerResult.Success();
        }
    }
}
