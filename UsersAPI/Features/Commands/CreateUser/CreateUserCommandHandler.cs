using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.Data.Entities;
using UsersAPI.Extensions;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Features.Commands.CreateUser
{
    public class CreateUserCommandHandler(
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
                    HandlerErrorType.Conflict,
                    $"User {dto.UserName} already exists");

            var user = _mapper.Map<User>(dto);

            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            //await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");

            _logger.LogInformation($"User {user.Username} was created");

            return HandlerResult<bool>.Success(true);
        }
    }
}
