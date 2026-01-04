using AutoMapper;
using Humanizer;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.Contracts.User.Events;
using Shared.Models.Contracts.User.Requests.CreateUser;
using Shared.Models.Contracts.User.Requests.DeleteUser;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.Extensions;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(
        IUserRepository _userRepository,
        IRequestClient<DeleteUserRequest> _client,
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

            bool isRequestSuccess = false;
            string requestErrorMessage = string.Empty;

            try
            {
                var requestResult = await _client.GetResponse<DeleteUserResponse>(new DeleteUserRequest
                {
                    Id = userId
                });

                isRequestSuccess = requestResult.Message.Success;
                requestErrorMessage = requestResult.Message.ErrorMessage;
            }
            catch (Exception ex)
            {
                requestErrorMessage = ex.Message;
            }

            if (!isRequestSuccess)
            {
                _logger.LogError($"Failed to delete user with Id {user.Id}. External service error: " + requestErrorMessage);
                throw new Exception("Error during deleting a user");
            }

            _userRepository.RemoveUser(user);
            if (!await _userRepository.SaveChangesAsync(cancellationToken))
                throw new Exception("Something wend wrong during deleting");

            await _cache.UpdateVersionAsync(CacheKeys.User.List);
            await _userCacheHelper.ClearUserCache(user);

            _logger.LogInformation($"User {user.Username} was deleted");

            return HandlerResult.Success();
        }
    }
}
