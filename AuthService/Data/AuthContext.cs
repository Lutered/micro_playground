using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AuthContext : IdentityDbContext<
        AppUser,
        AppRole,
        Guid,
        IdentityUserClaim<Guid>,
        AppUserRole, 
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, 
        IdentityUserToken<Guid>
    >
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuthContext(DbContextOptions options) : base(options) 
        { 
            //Database.EnsureDeleted(); 
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
