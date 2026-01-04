using System.Text.Json.Serialization;

namespace Shared.Models.DTOs.Course
{
    public class CourseDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; } = string.Empty;

        [JsonPropertyName("authorId")]
        public Guid AutorId { get; set; }

        [JsonPropertyName("authorName")]
        public string AutorName { get; set; } = string.Empty;
    }
}
