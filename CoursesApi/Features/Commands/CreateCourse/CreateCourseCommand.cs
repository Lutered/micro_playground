using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Course;

namespace CoursesApi.Features.Commands.CreateCourse
{
    public class CreateCourseCommand : IRequest<HandlerResult<Guid>>
    {
        public CreateCourseRequest Input { get; set; }

        public CreateCourseCommand(CreateCourseRequest input)
        {
            Input = input;
        }
    }
}
