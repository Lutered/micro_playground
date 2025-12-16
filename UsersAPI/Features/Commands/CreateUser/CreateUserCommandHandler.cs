using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.Extensions;

namespace UsersAPI.Features.Commands.CreateUser
{
    public class CreateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger<CreateUserCommandHandler> _logger
    )
    : IRequestHandler<CreateUserCommand, HandlerResult<Guid>>
    {
        public async Task<HandlerResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            if (await _userRepository.IsUserExists(input.Username))
                return HandlerResult<Guid>.Failure(
                    HandlerErrorType.Conflict,
                    $"User {input.Username} already exists");

            var user = _mapper.Map<User>(input);

            _userRepository.AddUser(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            //await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");

            _logger.LogInformation($"User {user.Username} was created");

            return HandlerResult<Guid>.Success(user.Id);
        }
    }
}
