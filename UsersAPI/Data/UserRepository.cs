using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Helpers;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Data
{
    public class UserRepository : IUserRepository
    {
        //Temporary cache implementation
        private readonly string _cachePrefix = "usersrep";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public UserRepository(
            AppDbContext context,
            IMapper mapper,
            IDistributedCache distributedCache
          )
        {
            _context = context;
            _mapper = mapper;
            _cache = distributedCache;
        }
        public async Task<PagedList<AppUserDTO>> GetUsersAsync(int page, int pageSize)
        {
            var version = await _cache.GetVersionAsync($"{_cachePrefix}:users");

            return await _cache.GetObjectOrCreateAsync<PagedList<AppUserDTO>>(
                $"{_cachePrefix}:users:{version}:{page}:{pageSize}", 
                async () => 
                {
                    int skipRecords = (page - 1) * pageSize;

                    var usersQuery = _context.Users
                                .Skip(skipRecords)
                                .Take(pageSize);

                    var query = usersQuery
                                .AsNoTracking()
                                .ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider);

                    return await PagedList<AppUserDTO>.CreateAsync(
                        query,
                        page,
                        pageSize);
                });
        }
        public async Task<AppUserDTO> GetUserAsync(string username)
        {
            return await _cache.GetObjectOrCreateAsync<AppUserDTO>(
                $"{_cachePrefix}:user:{username.ToString()}", 
                async () =>
                {
                    return _mapper.Map<User, AppUserDTO>(
                        await _context.Users.FirstOrDefaultAsync(x => x.Username == username)
                     );
                });
        }

        public async Task CreateUserAsync(AppUserDTO appUser)
        {
            var user = _mapper.Map<AppUserDTO, User>(appUser);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await _cache.UpdateVersionAsync($"{_cachePrefix}:users");
        }

        public async Task DeleteUserAsync(string username)
        {
            var user = _context.Users.First(x => x.Username == username);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            await _cache.UpdateVersionAsync($"{_cachePrefix}:users");
            await _cache.RemoveAsync($"{_cachePrefix}:user:{username.ToString()}");
        }
    }
}
