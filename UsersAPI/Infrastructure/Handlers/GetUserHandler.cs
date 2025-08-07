using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Infrastructure.Cache;
using UsersAPI.Infrastructure.Queries;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Infrastructure.Handlers
{
    public class GetUserHandler(
            IUserRepository _userRepository,
            IMapper _mapper,
            IDistributedCache _cache
        ) 
        : IRequestHandler<GetUserQuery, HandlerResult<AppUserDTO>>
    {
        public async Task<HandlerResult<AppUserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            string username = request.Username;

            string cacheKey = $"{CONSTS.USER_PREFIX}:user:{username}";
            var cachedUser = await _cache.GetAsync<AppUserDTO>(cacheKey);

            if (cachedUser is not null) 
                return HandlerResult<AppUserDTO>.Success(cachedUser);

            var user = await _userRepository.GetUserAsync(username, cancellationToken);

            if (user is null)
                return HandlerResult<AppUserDTO>.Failure(
                    new HandlerError("User was not found", HandlerErrorType.NotFound));

            var mappedUser = _mapper.Map<AppUserDTO>(user);

            await _cache.CreateAsync<AppUserDTO>(cacheKey, mappedUser);

            return HandlerResult<AppUserDTO>.Success(mappedUser);
        }
    }
}
