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
            CreateMap<Course, CourseDTO>()
                    .ForMember(desc => desc.AutorName, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));
            CreateMap<CreateCourseRequest, Course>()
                .ForMember(desc => desc.AutorId, opt => opt.MapFrom(src => Guid.Empty));
            CreateMap<UpdateCourseRequest, Course>();
        }
    }
}
