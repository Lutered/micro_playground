using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<
        AppUser,
        AppRole, 
        int,
        IdentityUserClaim<int>,
        AppUserRole, 
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>, 
        IdentityUserToken<int>
    >
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
    }
}
