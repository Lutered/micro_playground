using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;

namespace UsersAPI.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> IsUserExists(string username);
        public Task<UserDTO> GetUserAsync(Guid userId, CancellationToken cancellationToken);
        public Task<UserDTO> GetUserAsync(string username, CancellationToken cancellationToken);
        public Task<User> GetEntityAsync(Guid userId, CancellationToken cancellationToken);
        public Task<User> GetEntityAsync(string username, CancellationToken cancellationToken);
        public Task<PagedList<UserDTO>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken);

        public void AddUser(User user);
        public void RemoveUser(User user);
        public void UpdateUser(User user);
        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
