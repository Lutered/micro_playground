using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using UsersAPI.Data.Entities;
using UsersAPI.Extensions;
using UsersAPI.Infrastructure.Cache;
using UsersAPI.Infrastructure.Commands;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Infrastructure.Handlers
{
    public class CreateUserHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger _logger
    )
    : IRequestHandler<CreateUserCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DTO;

            if (await _userRepository.UserExists(dto.UserName))
                return HandlerResult<bool>.Failure(
                    new HandlerError($"User {dto.UserName} already exists"));

            var user = _mapper.Map<User>(dto);

            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");

            _logger.LogInformation($"User {user.Username} was created");

            return HandlerResult<bool>.Success(true);
        }
    }
}
