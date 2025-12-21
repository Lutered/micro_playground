using AutoMapper;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using Shared.Models.Contracts.User.PublishEvents;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.Extensions;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
        IUserRepository _userRepository,
        IPublishEndpoint _publishEndpoint,
        IDistributedCache _cache,
        ILogger<DeleteUserCommandHandler> _logger
    )
    : IRequestHandler<DeleteUserCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var userId = request.Id;
            var user = await _userRepository.GetEntityAsync(userId, cancellationToken);

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

           // await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");
           // await _cache.RemoveAsync($"{CONSTS.USER_PREFIX}:user:{username}");

            _logger.LogInformation($"User {user.Username} was deleted");

            return HandlerResult.Success();
        }
    }
}
