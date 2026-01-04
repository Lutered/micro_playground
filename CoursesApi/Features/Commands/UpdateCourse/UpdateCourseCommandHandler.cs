using AutoMapper;
using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using MassTransit;
using MediatR;
using Shared.Models.Common;
using Shared.Models.Contracts.User.Requests.GetUser;
using Shared.Models.DTOs.User;

namespace CoursesApi.Features.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler(
        ICourseRepository _courseRepo, 
        IParticipantsRepository _participantsRepo,
        IRequestClient<GetUserRequest> _client,
        IMapper _mapper
     ) : IRequestHandler<UpdateCourseCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var input = request.Input;

            var course = await _courseRepo.GetEntityAsync(id);

            if (course == null)
                return HandlerResult.Failure(HandlerErrorType.NotFound, $"Course with Id {id} was not found");

            var authorId = input.AutorId;
            var participant = await _participantsRepo.GetEntityAsync(authorId);

            if (participant is null)
            {
                UserDTO user;

                try
                {
                    var response = await _client.GetResponse<UserDTO>(new GetUserRequest(authorId));

                    if (response.Message == null)
                        return HandlerResult.Failure(HandlerErrorType.NotFound, $"User with id {authorId} was not found");

                    user = response.Message;

                    participant = new Participant()
                    {
                        UserId = user.Id,
                        FirstName = user.Username,
                    };

                    _participantsRepo.AddEntity(participant);
                }
                catch
                {
                    return HandlerResult.Failure(HandlerErrorType.Internal, "Error occurred when getting author by Id");
                }
            }

            course = _mapper.Map<Course>(input);

            await _courseRepo.SaveChangesAsync();

            return HandlerResult.Success();
        }
    }
}
