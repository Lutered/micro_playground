using CoursesApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesApi.Data
{
    public class CourseContext : DbContext
    {
        public CourseContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted(); 
            //Database.EnsureCreated();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Participant> Participants { get; set; }
    }
}
