
using AuthAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
          => await _context.Users.FindAsync(id);

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
