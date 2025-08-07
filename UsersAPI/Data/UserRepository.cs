using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Helpers;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Data
{
    public class UserRepository(
        AppDbContext _context,
        IMapper _mapper
    ) : IUserRepository
    {
        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }
        public async Task<User> GetUserAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
        }

        public async Task<PagedList<AppUserDTO>> GetUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
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
