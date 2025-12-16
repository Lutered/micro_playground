using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.DTOs;
using UsersAPI.Extensions;

namespace UsersAPI.Features.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger<UpdateUserCommandHandler> _logger
    )
    : IRequestHandler<UpdateUserCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var input = request.Input;

            var user = await _userRepository.GetEntityAsync(id, cancellationToken);

            if (user is null)
                return HandlerResult.Failure(
                    HandlerErrorType.NotFound, 
                    $"User with Id {id} does not exists");

            await _userRepository.SaveChangesAsync(cancellationToken);

            //await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");
            //await _cache.RemoveAsync($"{CONSTS.USER_PREFIX}:user:{user.Username}");

            _logger.LogInformation($"User {user.Username} was updated");

            return HandlerResult.Success();
        }
    }
}
