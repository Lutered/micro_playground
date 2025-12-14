using AutoMapper;
using CoursesApi.Data.Entities;
using Shared.Models.DTOs.Course;
using Shared.Models.Requests.Course;

namespace CoursesApi.Mappers
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile() 
        {
            CreateMap<Course, CourseDTO>();
            CreateMap<CreateCourseRequest, Course>();
            CreateMap<UpdateCourseRequest, Course>();
        }
    }
}
