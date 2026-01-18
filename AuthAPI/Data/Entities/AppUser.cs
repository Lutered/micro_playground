using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public int Age { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
