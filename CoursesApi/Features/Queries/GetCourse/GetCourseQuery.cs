using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;

namespace CoursesApi.Features.Queries.GetCourse
{
    public class GetCourseQuery : IRequest<HandlerResult<CourseDTO>>
    {
        public Guid Id { get; private set; }

        public GetCourseQuery(Guid id) { Id = id; }
    }
}
