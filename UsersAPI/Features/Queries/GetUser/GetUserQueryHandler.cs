using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Helpers;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Features.Queries.GetUser
{
    public class GetUserQueryHandler(
            IUserRepository _userRepository,
            IMapper _mapper,
            IDistributedCache _cache
        ) 
        : IRequestHandler<GetUserQuery, HandlerResult<AppUserDTO>>
    {
        public async Task<HandlerResult<AppUserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            Guid? id = request.Id;
            string username = request.Username;

            string cacheKey = id != null ? 
                $"{CACHEKEYS.USER_KEY}:user_id:{id.ToString()}" :
                $"{CACHEKEYS.USER_KEY}:user_name:{username}";

            var cachedUser = await _cache.GetAsync<AppUserDTO>(cacheKey);

            if (cachedUser is not null)
                return HandlerResult<AppUserDTO>.Success(cachedUser);

            var user = id != null ?
                await _userRepository.GetUserAsync((Guid)id, cancellationToken) :
                await _userRepository.GetUserAsync(username, cancellationToken);

            if (user is null)
                return HandlerResult<AppUserDTO>.Failure(HandlerErrorType.NotFound, "User was not found");

            var mappedUser = _mapper.Map<AppUserDTO>(user);

            await _cache.CreateAsync($"{CACHEKEYS.USER_KEY}:user_id:{user.Id.ToString()}", mappedUser);
            await _cache.CreateAsync($"{CACHEKEYS.USER_KEY}:user_name:{user.Username.ToString()}", mappedUser);

            return HandlerResult<AppUserDTO>.Success(mappedUser);
        }
    }
}
