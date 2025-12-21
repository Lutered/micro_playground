using CoursesApi.Data;
using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using MassTransit;
using MediatR;
using Shared.Models.Common;
using Shared.Models.Contracts.User.Requests;
using Shared.Models.DTOs.User;

namespace CoursesApi.Features.Commands.AddToCourse
{
    public class AddToCourseCommandHandler(
        IRequestClient<GetUserRequest> _client,
        ICourseRepository _courseRepo,
        IParticipantsRepository _participantsRepo
    ) : IRequestHandler<AddToCourseCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(AddToCourseCommand request, CancellationToken cancellationToken)
        {
            var courseId = request.CourseId;
            var userId = request.UserId;

            var course = await _courseRepo.GetEntityAsync(courseId, cancellationToken);

            if (course == null)
                return HandlerResult.Failure(HandlerErrorType.NotFound, $"Course with Id {courseId} was not found");

            var participant = await _participantsRepo.GetEntityByUserIdAsync(userId, cancellationToken);

            if(participant == null)
            {
                var response = await _client.GetResponse<UserDTO>(new GetUserRequest(userId));

                if (response.Message == null)
                    return HandlerResult.Failure(HandlerErrorType.NotFound, $"User with id {userId} was not found");

                var user = response.Message;

                participant = new Participant()
                {
                    UserId = user.Id,
                    FirstName = user.Username,
                };

                _participantsRepo.AddEntity(participant);
            }

            course.Students.Add(participant);

            await _courseRepo.SaveChangesAsync(cancellationToken);

            return HandlerResult.Success();
        }
    }
}
