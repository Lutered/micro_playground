using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using CoursesApi.Data;

namespace AuthAPI.Data
{
    public class CourseContextFactory : IDesignTimeDbContextFactory<CourseContext>
    {
        public CourseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CourseContext>();
            optionsBuilder.UseNpgsql();

            return new CourseContext(optionsBuilder.Options);
        }
    }
}
