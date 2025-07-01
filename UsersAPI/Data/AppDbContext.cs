using Microsoft.EntityFrameworkCore;
using UsersAPI.Data.Entities;

namespace UsersAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted(); 
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}
