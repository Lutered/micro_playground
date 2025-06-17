using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        public required AppUser User { get; set; }
        public required AppRole Role { get; set; }
    }
}
