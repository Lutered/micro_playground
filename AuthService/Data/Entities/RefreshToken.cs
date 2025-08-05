using Microsoft.AspNetCore.Identity;

namespace AuthAPI.Data.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime Expires { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
