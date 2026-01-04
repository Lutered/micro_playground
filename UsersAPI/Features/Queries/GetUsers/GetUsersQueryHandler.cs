using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Configurations;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;
using System.Linq.Expressions;
using UsersAPI.Data.Repositories.Interfaces;

namespace UsersAPI.Features.Queries.GetUsers
{
    public class GetUsersQueryHandler(
            IUserRepository _userRepository,
            IMapper _mapper,
            IDistributedCache _cache
        )
        : IRequestHandler<GetUsersQuery, HandlerResult<PagedList<UserDTO>>>
    {
        Dictionary<string, Expression<Func<UserDTO, object>>> sortMap = new()
        {
            ["Id"] = x => x.Id,
            ["Username"] = x => x.Username,
            ["Email"] = x => x.Email,
            ["Age"] = x => x.Age
        };

        public async Task<HandlerResult<PagedList<UserDTO>>> Handle(GetUsersQuery request, CancellationToken cancellationToken = default)
        {
            var input = request.Input;

            var version = await _cache.GetVersionAsync($"{CacheKeys.User.List}");

            string cacheKey = CacheHelper.GenerateCacheKey($"{CacheKeys.User.List}:{version}", input);
            var cachedUsers = await _cache.GetAsync<PagedList<UserDTO>>(cacheKey);

            if (cachedUsers is not null) 
                return HandlerResult<PagedList<UserDTO>>.Success(cachedUsers);

            var users = await GetPagedUsersAsync(input, cancellationToken);

            await _cache.CreateAsync(cacheKey, users);

            return HandlerResult<PagedList<UserDTO>>.Success(users);
        }

        private async Task<PagedList<UserDTO>> GetPagedUsersAsync(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetBaseQuery()
                        .ProjectTo<UserDTO>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(request.Username))
                query = query.Where(x => EF.Functions.Like(x.Username.ToLower(), $"%{request.Username.ToLower()}%"));

            if (!string.IsNullOrEmpty(request.Email))
                query = query.Where(x => EF.Functions.Like(x.Email.ToLower(), $"%{request.Email.ToLower()}%"));

            if (request.StartAge.HasValue)
                query = query.Where(x => x.Age >= request.StartAge);

            if (request.EndAge.HasValue)
                query = query.Where(x => x.Age <= request.EndAge);

            if (!string.IsNullOrWhiteSpace(request.Sort))
                query = query.ApplySort(request.Sort, sortMap);

            return await query.ToPagedListAsync(request.Page, request.PageSize);
        }
    }
}
