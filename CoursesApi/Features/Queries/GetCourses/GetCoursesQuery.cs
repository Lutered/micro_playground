using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;
using Shared.Models.Requests;
using Shared.Models.Requests.Course;

namespace CoursesApi.Features.Queries.GetCourses
{
    public class GetCoursesQuery : IRequest<HandlerResult<PagedList<CourseDTO>>>
    {
        public GetCoursesRequest Input { get; private set; }

        public GetCoursesQuery(GetCoursesRequest input)
        {
            Input = input;
        }
    }
}
