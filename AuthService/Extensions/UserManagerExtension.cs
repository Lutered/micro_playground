using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<bool> UserExists(this UserManager<AppUser> _userManager, string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
