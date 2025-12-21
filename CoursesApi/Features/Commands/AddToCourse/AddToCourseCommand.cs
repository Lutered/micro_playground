using MediatR;
using Shared.Models.Common;

namespace CoursesApi.Features.Commands.AddToCourse
{
    public class AddToCourseCommand : IRequest<HandlerResult>
    {
        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        public AddToCourseCommand(Guid userID, Guid courseId)
        {
            UserId = userID;
            CourseId = courseId;
        }
    }
}
