using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.Models.Common;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Data.Repositories
{
    public class UserRepository(
        UserContext _context,
        IMapper _mapper
    ) : IUserRepository
    {
        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
        public async Task<User> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }
        public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        }

        public async Task<PagedList<AppUserDTO>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Users
                        .AsNoTracking()
                        .ProjectTo<AppUserDTO>(_mapper.ConfigurationProvider)
                        .ToPagedListAsync(page, pageSize);
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }

        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
