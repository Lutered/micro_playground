using AutoMapper;
using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using MassTransit;
using MediatR;
using Shared.Models.Common;
using Shared.Models.Contracts.User.Requests.GetUser;
using Shared.Models.DTOs.User;

namespace CoursesApi.Features.Commands.CreateCourse
{
    public class CreateCourseCommandHandler(
        ICourseRepository _courseRepo, 
        IParticipantsRepository _participantsRepo,
        IMapper _mapper, 
        IRequestClient<GetUserRequest> _client
    ) : IRequestHandler<CreateCourseCommand, HandlerResult<Guid>>
    {
        public async Task<HandlerResult<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var input = request.Input;

            var authorId = input.AutorId;

            var participant = await _participantsRepo.GetEntityAsync(authorId);

            if(participant is null)
            {
                UserDTO user;

                try
                {
                    var response = await _client.GetResponse<UserDTO>(new GetUserRequest(authorId));

                    if (response.Message == null)
                        return HandlerResult<Guid>.Failure(HandlerErrorType.NotFound, $"User with id {authorId} was not found");

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
                    return HandlerResult<Guid>.Failure(HandlerErrorType.Internal, "Error occurred when getting author by Id");
                }
            }

            var courseEntity = _mapper.Map<Course>(input);
            courseEntity.Author = participant;

            _courseRepo.AddEntity(courseEntity);
            bool isSaved = await _courseRepo.SaveChangesAsync();

            if (!isSaved) 
                return HandlerResult<Guid>.Failure(HandlerErrorType.Internal, "Error occured during saving course");

            return HandlerResult<Guid>.Success(courseEntity.Id);
        }
    }
}
