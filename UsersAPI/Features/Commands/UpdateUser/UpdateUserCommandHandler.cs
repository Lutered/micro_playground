using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Features.Commands.UpdateUser
{
    public class UpdateUserCommandHandler(
        IUserRepository _userRepository,
        IMapper _mapper,
        IDistributedCache _cache,
        ILogger _logger
    )
    : IRequestHandler<UpdateUserCommand, HandlerResult<bool>>
    {
        public async Task<HandlerResult<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.DTO;

            var user = await _userRepository.GetUserAsync(dto.UserName, cancellationToken);

            if (user is null)
                return HandlerResult<bool>.Failure(
                    HandlerErrorType.NotFound, 
                    $"User {dto.UserName} does not exists");

            _mapper.Map(dto, user);

            await _userRepository.SaveChangesAsync(cancellationToken);

            //await _cache.UpdateVersionAsync($"{CONSTS.USER_PREFIX}:users");
            //await _cache.RemoveAsync($"{CONSTS.USER_PREFIX}:user:{user.Username}");

            _logger.LogInformation($"User {user.Username} was updated");

            return HandlerResult<bool>.Success(true);
        }
    }
}
