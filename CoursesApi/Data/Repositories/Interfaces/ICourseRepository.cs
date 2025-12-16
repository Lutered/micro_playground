using CoursesApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Common;
using Shared.Models.DTOs.Course;
using Shared.Models.Requests.Course;
using System.Threading;

namespace CoursesApi.Data.Repositories.Interfaces
{
    public interface ICourseRepository
    {
        public Task<PagedList<CourseDTO>> GetAllCoursesAsync(
            GetCoursesRequest request,
            CancellationToken cancellationToken = default
        );

        public Task<CourseDTO?> GetCourseAsync(Guid id, CancellationToken cancellationToken = default);

        public Task<Course?> GetEntityAsync(Guid id, CancellationToken cancellationToken = default);

        public void AddEntity(Course course);

        public void RemoveEntity(Course course);

        public void UpdateEntity(Course course);

        public Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
