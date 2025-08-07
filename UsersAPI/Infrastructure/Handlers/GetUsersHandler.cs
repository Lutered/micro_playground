using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Helpers;
using UsersAPI.Infrastructure.Cache;
using UsersAPI.Infrastructure.Queries;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Infrastructure.Handlers
{
    public class GetUsersHandler(
            IUserRepository _userRepository,
            IDistributedCache _cache
        )
        : IRequestHandler<GetUsersQuery, HandlerResult<PagedList<AppUserDTO>>>
    {
        public async Task<HandlerResult<PagedList<AppUserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var pageParams = request.PageParams;

            var version = await _cache.GetVersionAsync($"{CONSTS.USER_PREFIX}:users");

            string cacheKey = $"{CONSTS.USER_PREFIX}:users:{version}:{pageParams.Page}:{pageParams.PageSize}";
            var cachedUsers = await _cache.GetAsync<PagedList<AppUserDTO>>(cacheKey);

            if (cachedUsers is not null)
                return HandlerResult<PagedList<AppUserDTO>>.Success(cachedUsers);

            var users = await _userRepository.GetUsersAsync(pageParams.Page, pageParams.PageSize, cancellationToken);

            await _cache.CreateAsync<PagedList<AppUserDTO>>(cacheKey, users);

            return HandlerResult<PagedList<AppUserDTO>>.Success(users);
        }
    }
}
