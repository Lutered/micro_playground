using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Helpers;
using UsersAPI.Interfaces;

namespace UsersAPI.Data
{
    public class UserRepository
    {
        private readonly string cachePrefix = "users_";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;
        public UserRepository(AppDbContext context, IMapper mapper, ICacheService distributedCache)
        {
            _context = context;
            _mapper = mapper;
            _cache = distributedCache;
        }
        public async Task<PagedList<AppUserDTO>> GetUsersAsync(int page = 1, int pageSize = 25)
        {
            //var cacheUsers = await _cache.GetStringAsync($"{cachePrefix}/users/{page}/{pageSize}");

            //if (cacheUsers != null) return JsonSerializer.Deserialize<PagedList<AppUserDTO>>(cacheUsers);

            //int skipRecords = (page - 1) * pageSize;

            //var usersQuery = _context.Users
            //            .Skip(skipRecords)
            //            .Take(pageSize);

            //var users = await PagedList<AppUserDTO>.CreateAsync(
            //    usersQuery.AsNoTracking().ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider),
            //    page,
            //    pageSize);

            //cacheUsers = JsonSerializer.Serialize(users);

            //await _cache.SetStringAsync($"{cachePrefix}/users/{page}/{pageSize}", cacheUsers, new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            //});

            //return users;

            var version = _cache.GetCacheObjectOrCreateAsync<int>($"users:version", async () => { return 1; });

            return await _cache.GetCacheObjectOrCreateAsync<PagedList<AppUserDTO>>($"{cachePrefix}:users:{version}:{page}:{pageSize}", async () => 
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
            //User? user = null;

            //var cacheUser = await _cache.GetStringAsync($"{cachePrefix}/user/" + username.ToString());

            //if (cacheUser != null) user = JsonSerializer.Deserialize<User>(cacheUser);

            //if(user == null)
            //{
            //    user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            //    if (user != null)
            //    {
            //        cacheUser = JsonSerializer.Serialize(user);

            //        await _cache.SetStringAsync(cachePrefix + "/user/" + user.UserName.ToString(), cacheUser, new DistributedCacheEntryOptions
            //        {
            //            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            //        });
            //    }
            //}

            //return _mapper.Map<User, AppUserDTO>(user);

            return await _cache.GetCacheObjectOrCreateAsync<AppUserDTO>($"{cachePrefix}:user:" + username.ToString(), async () =>
            {
                return _mapper.Map<User, AppUserDTO>(
                    await _context.Users.FirstOrDefaultAsync(x => x.UserName == username)
                 );
            });
        }

        public async Task CreateUserAsync(AppUserDTO appUser)
        {
            var user = _mapper.Map<AppUserDTO, User>(appUser);

            var versionStr = await _cache.GetCacheRawDataAsync("users:version");

            if(versionStr != null && Int32.TryParse(versionStr, out int version))
            {
                await _cache.UpdateOrCreateRawDataAsync("users:version", (versionStr + 1).ToString());
            }

            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
