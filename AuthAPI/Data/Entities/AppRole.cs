using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }

        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
    }
}
