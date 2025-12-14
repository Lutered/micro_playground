namespace CoursesApi.Data.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

        public Guid AutorId { get; set; }
        public required Participant Author { get; set; }

        public List<Participant> Students { get; set; } = new();
    }
}
