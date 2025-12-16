using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;
using Shared.Models.DTOs.User;
using Shared.Models.Requests.User;
using System.Linq.Expressions;
using System.Threading;
using UsersAPI.Data.Entities;
using UsersAPI.Data.Repositories.Interfaces;
using UsersAPI.DTOs;

namespace UsersAPI.Data.Repositories
{
    public class UserRepository(
        UserContext _context,
        IMapper _mapper
    ) : IUserRepository
    {
        Dictionary<string, Expression<Func<UserDTO, object>>> sortMap = new()
        {
            // ["Id"] = x => x.Id
        };

        public async Task<bool> IsUserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username);
        }

        public async Task<PagedList<UserDTO>> GetUsersAsync(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Users
                        .ProjectTo<UserDTO>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(request.Sort)) 
                query = query.ApplySort(request.Sort, sortMap);

            return await query.ToPagedListAsync(request.Page, request.PageSize);
        }

        public async Task<UserDTO> GetUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                        .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task<UserDTO> GetUserAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users
                        .Where(x => x.Username == username)
                        .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<User> GetEntityAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }
        public async Task<User> GetEntityAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users
                            .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
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
