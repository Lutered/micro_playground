using Shared.Models.Common;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;

namespace UsersAPI.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> UserExists(string username);
        public Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task<User> GetUserAsync(string username, CancellationToken cancellationToken);
        public Task<PagedList<AppUserDTO>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken);

        public void AddUser(User user);
        public void RemoveUser(User user);
        public void UpdateUser(User user);
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
