using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests.Course
{
    public class UpdateCourseRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public Guid AutorId { get; set; }
    }
}
