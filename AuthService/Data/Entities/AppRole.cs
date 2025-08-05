using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }

        public AppRole() : base() { }
        public AppRole(string name) : base(name) { }
    }
}
