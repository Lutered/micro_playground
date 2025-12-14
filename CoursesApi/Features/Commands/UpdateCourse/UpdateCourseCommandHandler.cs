using AutoMapper;
using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using MediatR;
using Shared.Models.Common;

namespace CoursesApi.Features.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler(ICourseRepository _courseRepo, IMapper _mapper)
        : IRequestHandler<UpdateCourseCommand, HandlerResult>
    {
        public async Task<HandlerResult> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var id = request.Id;
            var input = request.Input;

            var course = await _courseRepo.GetEntityAsync(id);

            course = _mapper.Map<Course>(input);

            await _courseRepo.SaveChangesAsync();

            return HandlerResult.Success();
        }
    }
}
