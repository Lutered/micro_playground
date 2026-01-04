using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.Extensions;
using UsersAPI.Helpers;

namespace UsersAPI.Features.Commands.CreateUser
{
    public class CreateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger<CreateUserCommandHandler> _logger
    )
    : IRequestHandler<CreateUserCommand, HandlerResult<UserDTO>>
    {
        public async Task<HandlerResult<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            if (await _userRepository.IsUserExists(input.Username))
                return HandlerResult<UserDTO>.Failure(
                    HandlerErrorType.Conflict,
                    $"User {input.Username} already exists");

            var user = _mapper.Map<User>(input);

            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            await _cache.UpdateVersionAsync(CacheKeys.User.List);

            _logger.LogInformation($"User {user.Username} was created");

            var createdUser = await _userRepository.GetUserAsync(user.Id, cancellationToken);

            return HandlerResult<UserDTO>.Success(_mapper.Map<UserDTO>(createdUser));
        }
    }
}
