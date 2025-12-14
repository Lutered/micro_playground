using MediatR;
using Shared.Models.Common;

namespace CoursesApi.Features.Commands.DeleteCourse
{
    public class DeleteCourseCommand : IRequest<HandlerResult>
    {
        public Guid Id { get; set; }

        public DeleteCourseCommand(Guid id)
        {
            Id = id;
        }
    }
}
