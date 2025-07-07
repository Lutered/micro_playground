using Microsoft.EntityFrameworkCore;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Helpers;

namespace UsersAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<PagedList<AppUserDTO>> GetUsersAsync(int page, int pageSize);
        public Task<AppUserDTO> GetUserAsync(string username);
        public Task CreateUserAsync(AppUserDTO appUser);
        public Task UpdateCache();
        public void Update(User user);
        public Task<int> SaveChangesAsync();
    }
}
