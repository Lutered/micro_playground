
namespace Shared.Models.Requests.Course
{
    public class CreateCourseRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid AutorId { get; set; }
    }
}
