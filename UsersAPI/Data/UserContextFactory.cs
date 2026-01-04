using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using UsersAPI.Data;

namespace UserAPI.Data
{
    public class UserContextFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseNpgsql();

            return new UserContext(optionsBuilder.Options);
        }
    }
}

