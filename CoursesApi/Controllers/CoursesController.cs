using CoursesApi.Features.Commands.CreateCourse;
using CoursesApi.Features.Commands.DeleteCourse;
using CoursesApi.Features.Commands.UpdateCourse;
using CoursesApi.Features.Queries.GetCourse;
using CoursesApi.Features.Queries.GetCourses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Requests;
using Shared.Models.Requests.Course;
using System.IdentityModel.Tokens.Jwt;

namespace CoursesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoursesController(IMediator _mediator): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCourses(
            PagedRequest request, 
            CancellationToken cancellationToken = default)
        {
            var query = new GetCoursesQuery(request);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("mycourses")]
        public async Task<IActionResult> GetMyCourses(PagedRequest request, CancellationToken cancellationToken = default)
        {
            string userIdStr = User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;

            if (!Guid.TryParse(userIdStr, out Guid userId))
                return BadRequest("User Id was not found");

            var query = new GetCourseQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new GetCourseQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateCourseCommand(request);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(Guid id, UpdateCourseRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCourseCommand(id, request);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteCourseCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost("addMe/{courseId}")]
        public async Task<IActionResult> AddToCourse(Guid courseId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("removeMe/{courseId}")]
        public async Task<IActionResult> RemoveFromCourse(Guid courseId)
        {
            throw new NotImplementedException();
        }
    }
}
