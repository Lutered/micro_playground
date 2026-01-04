using AutoMapper;
using MassTransit;
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

namespace UsersAPI.Features.Queries.GetUser
{
    public class GetUserQueryHandler(
            IUserRepository _userRepository,
            IMapper _mapper,
            IDistributedCache _cache
        ) 
        : IRequestHandler<GetUserQuery, HandlerResult<UserDTO>>
    {
        public async Task<HandlerResult<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            Guid? id = request.Id;
            string username = request.Username;

            string cacheKey = id != null ? 
                $"{CacheKeys.User.UserId}:{id.ToString()}" :
                $"{CacheKeys.User.UserName}:{username}";

            var cachedUser = await _cache.GetAsync<UserDTO>(cacheKey);

            if (cachedUser is not null)
                return HandlerResult<UserDTO>.Success(cachedUser);

            var user = id != null ?
                await _userRepository.GetUserAsync((Guid)id, cancellationToken) :
                await _userRepository.GetUserAsync(username, cancellationToken);

            if (user is null)
                return HandlerResult<UserDTO>.Failure(HandlerErrorType.NotFound, "User was not found");

            var userDto = _mapper.Map<UserDTO>(user);

            await _cache.CreateAsync($"{CacheKeys.User.UserId}:{user.Id.ToString()}", userDto);
            await _cache.CreateAsync($"{CacheKeys.User.UserName}:{user.Username.ToString()}", userDto);

            return HandlerResult<UserDTO>.Success(userDto);
        }
    }
}
