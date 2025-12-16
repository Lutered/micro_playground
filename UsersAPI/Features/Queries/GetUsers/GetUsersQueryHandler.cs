using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.DTOs;

namespace UsersAPI.Features.Queries.GetUsers
{
    public class GetUsersQueryHandler(
            IUserRepository _userRepository,
            IDistributedCache _cache
        )
        : IRequestHandler<GetUsersQuery, HandlerResult<PagedList<UserDTO>>>
    {
        public async Task<HandlerResult<PagedList<UserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            //var version = await _cache.GetVersionAsync($"{CONSTS.USER_PREFIX}:users");

            //string cacheKey = $"{CONSTS.USER_PREFIX}:users:{version}:{pageParams.Page}:{pageParams.PageSize}";
            //var cachedUsers = await _cache.GetAsync<PagedList<AppUserDTO>>(cacheKey);

            //if (cachedUsers is not null)
            //    return HandlerResult<PagedList<AppUserDTO>>.Success(cachedUsers);

            var users = await _userRepository.GetUsersAsync(input, cancellationToken);

            //await _cache.CreateAsync(cacheKey, users);

            return HandlerResult<PagedList<UserDTO>>.Success(users);
        }
    }
}
