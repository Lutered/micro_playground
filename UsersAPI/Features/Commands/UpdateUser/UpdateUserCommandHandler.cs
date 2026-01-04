using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger<UpdateUserCommandHandler> _logger,
        UserCacheHelper _userCacheHelper
    )
    : IRequestHandler<UpdateUserCommand, HandlerResult<UserDTO>>
    {
        public async Task<HandlerResult<UserDTO>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var input = request.Input;

            var user = await _userRepository.GetUserAsync(id, cancellationToken);

            if (user is null)
                return HandlerResult<UserDTO>.Failure(
                    HandlerErrorType.NotFound, 
                    $"User with Id {id} does not exists");

            await _userRepository.SaveChangesAsync(cancellationToken);

            await _cache.UpdateVersionAsync(CacheKeys.User.List);
            await _userCacheHelper.ClearUserCache(user);

            _logger.LogInformation($"User {user.Username} was updated");

            var updatedUser = await _userRepository.GetUserAsync(user.Id, cancellationToken);

            return HandlerResult<UserDTO>.Success(_mapper.Map<UserDTO>(updatedUser));
        }
    }
}
