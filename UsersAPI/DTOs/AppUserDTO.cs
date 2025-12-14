namespace UsersAPI.DTOs
{
    public class AppUserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
