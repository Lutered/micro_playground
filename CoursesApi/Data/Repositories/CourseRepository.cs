using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoursesApi.Data.Entities;
using CoursesApi.Data.Repositories.Interfaces;
using Elasticsearch.Net;
using Shared.Extensions;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;
using Shared.Models.Requests.Course;
using System.Linq.Expressions;

namespace CoursesApi.Data.Repositories
{
    public class CourseRepository(
        CourseContext _context, 
        IMapper _mapper) : ICourseRepository
    {
        Dictionary<string, Expression<Func<CourseDTO, object>>> sortMap = new()
        {
           // ["Id"] = x => x.Id
        };

        public async Task<PagedList<CourseDTO>> GetAllCoursesAsync(
             GetCoursesRequest request,
             CancellationToken cancellationToken = default
         ) 
        {
            var query = _context.Courses
                            .ProjectTo<CourseDTO>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrWhiteSpace(request.Sort)) 
                query = query.ApplySort(request.Sort, sortMap);

            return await query.ToPagedListAsync(request.Page, request.PageSize);
        }

        public async Task<CourseDTO?> GetCourseAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var course = await _context.Courses.FindAsync(id, cancellationToken);

            return course != null ? _mapper.Map<CourseDTO>(course) : null;
        }

        public async Task<Course?> GetEntityAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Courses.FindAsync(id, cancellationToken);
        }

        public void AddEntity(Course course)
        {
            _context.Courses.Add(course);
        }

        public void RemoveEntity(Course course)
        {
            _context.Courses.Remove(course);
        }

        public void UpdateEntity(Course course)
        {
            _context.Courses.Update(course);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
