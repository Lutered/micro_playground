using MediatR;
using Shared.Models.Common;

namespace CoursesApi.Features.Commands.RemoveFromCourse
{
    public class RemoveFromCourseCommand : IRequest<HandlerResult>
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        public RemoveFromCourseCommand(Guid userID, Guid courseId)
        {
            UserId = userID;
            CourseId = courseId;
        }
    }
}
