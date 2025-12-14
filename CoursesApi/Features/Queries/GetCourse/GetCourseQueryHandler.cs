using AutoMapper;
using CoursesApi.Data.Repositories.Interfaces;
using MediatR;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;

namespace CoursesApi.Features.Queries.GetCourse
{
    public class GetCourseQueryHandler(ICourseRepository _courseRepo, IMapper _mapper)
        : IRequestHandler<GetCourseQuery, HandlerResult<CourseDTO>>
    {
        public async Task<HandlerResult<CourseDTO>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepo.GetCourseAsync(request.Id);

            if (course == null) 
                return HandlerResult<CourseDTO>.Failure(HandlerErrorType.NotFound, "Course was not found");

            return HandlerResult<CourseDTO>.Success(course);
        }
    }
}
