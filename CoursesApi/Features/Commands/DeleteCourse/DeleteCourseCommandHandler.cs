using CoursesApi.Data.Repositories.Interfaces;
using MediatR;
using Shared.Models.Common;

namespace CoursesApi.Features.Commands.DeleteCourse
{
    public class DeleteCourseCommandHandler(ICourseRepository _courseRepo)
        : IRequestHandler<DeleteCourseCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;

            var course = await _courseRepo.GetEntityAsync(id);

            if(course == null)
                return HandlerResult.Failure(HandlerErrorType.NotFound, $"Course with {id} was not found");

            _courseRepo.RemoveEntity(course);

            bool isSaved = await _courseRepo.SaveChangesAsync();

            if (isSaved) 
                return HandlerResult.Failure(HandlerErrorType.Internal, "Error occurred during deleting course");

            return HandlerResult.Success();
        }
    }
}
