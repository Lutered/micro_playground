using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models.Common;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
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
            string username = request.Username;

            //string cacheKey = $"{CONSTS.USER_PREFIX}:user:{username}";
            //var cachedUser = await _cache.GetAsync<AppUserDTO>(cacheKey);

            //if (cachedUser is not null) 
            //    return HandlerResult<AppUserDTO>.Success(cachedUser);

            var user = await _userRepository.GetUserAsync(username, cancellationToken);

            if (user is null)
                return HandlerResult<AppUserDTO>.Failure(HandlerErrorType.NotFound, "User was not found");

            var mappedUser = _mapper.Map<AppUserDTO>(user);

            //await _cache.CreateAsync(cacheKey, mappedUser);

            return HandlerResult<AppUserDTO>.Success(mappedUser);
        }
    }
}
