using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public required AppUser User { get; set; }
        public required AppRole Role { get; set; }
    }
}
