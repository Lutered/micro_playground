namespace AuthAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
