using Microsoft.AspNetCore.SignalR;

namespace CoursesApi.Data.Entities
{
    public class Participant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
