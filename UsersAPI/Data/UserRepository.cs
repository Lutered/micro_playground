using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Extensions;
using UsersAPI.Helpers;
using UsersAPI.Interfaces;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string cachePrefix = "usersrep";

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
            var version = await _cache.GetVersionAsync($"{cachePrefix}:users");

            return await _cache.GetObjectOrCreateAsync<PagedList<AppUserDTO>>($"{cachePrefix}:users:{version}:{page}:{pageSize}", 
                async () => 
                {
                    int skipRecords = (page - 1) * pageSize;

                    var usersQuery = _context.Users
                                .Skip(skipRecords)
                                .Take(pageSize);

                    return await PagedList<AppUserDTO>.CreateAsync(
                        usersQuery.AsNoTracking().ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider),
                        page,
                        pageSize);
                });
        }
        public async Task<AppUserDTO> GetUserAsync(string username)
        {
            return await _cache.GetObjectOrCreateAsync<AppUserDTO>($"{cachePrefix}:user:{username.ToString()}", 
                async () =>
                {
                    return _mapper.Map<User, AppUserDTO>(
                        await _context.Users.FirstOrDefaultAsync(x => x.UserName == username)
                     );
                });
        }

        public async Task CreateUserAsync(AppUserDTO appUser)
        {
            var user = _mapper.Map<AppUserDTO, User>(appUser);

            await _context.Users.AddAsync(user);
        }

        public async Task UpdateCache()
        {
            await _cache.UpdateVersionAsync($"{cachePrefix}:users");
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            var result = await _context.SaveChangesAsync();
            await UpdateCache();

            return result;
        }
    }
}
