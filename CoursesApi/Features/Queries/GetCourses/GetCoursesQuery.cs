using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;
using Shared.Models.Requests;

namespace CoursesApi.Features.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<HandlerResult<PagedList<CourseDTO>>>
    {
        public PagedRequest Input { get; private set; }

        public GetCoursesQuery(PagedRequest input)
        {
            Input = input;
        }
    }
}
