using Shared.Models.Common;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;
using UsersAPI.Data.Entities;

namespace UsersAPI.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> IsUserExists(string username);
        public IQueryable GetBaseQuery();
        public Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task<User> GetUserAsync(string username, CancellationToken cancellationToken);

        public void AddUser(User user);
        public void RemoveUser(User user);
        public void UpdateUser(User user);
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
