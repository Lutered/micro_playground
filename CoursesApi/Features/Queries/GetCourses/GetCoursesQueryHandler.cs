using CoursesApi.Data.Repositories.Interfaces;
using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;

namespace CoursesApi.Features.Queries.GetCourses
{
    public class GetCoursesQueryHandler(ICourseRepository _courseRepo) 
        : IRequestHandler<GetCoursesQuery, HandlerResult<PagedList<CourseDTO>>>
    {
        public async Task<HandlerResult<PagedList<CourseDTO>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken = default)
        {
            var input = request.Input;

            var courses = await _courseRepo.GetAllCoursesAsync(input, cancellationToken);

            return HandlerResult<PagedList<CourseDTO>>.Success(courses);
        }
    }
}
