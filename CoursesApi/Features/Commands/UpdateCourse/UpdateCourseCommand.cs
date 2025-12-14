using MediatR;
using Shared.Models.Common;
using Shared.Models.Requests.Course;

namespace CoursesApi.Features.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<HandlerResult>
    {
        public Guid Id { get; set; }
        public UpdateCourseRequest Input { get; set; }

        public UpdateCourseCommand(Guid id, UpdateCourseRequest input) 
        {
            Id = id;
            Input = input;
        }
    }
}
