using AutoMapper;
using SIS.Application.DTOs;
using SIS.Web.ViewModels;

namespace SIS.Web.Mappings;

public class WebMappingProfile : Profile
{
    public WebMappingProfile()
    {
        // Student mappings
        CreateMap<StudentDto, StudentViewModel>();
        CreateMap<StudentDto, EditStudentViewModel>();
        CreateMap<CreateStudentViewModel, CreateStudentDto>();
        CreateMap<EditStudentViewModel, UpdateStudentDto>();

        // Course mappings
        CreateMap<CourseDto, CourseViewModel>();
        CreateMap<CourseDto, EditCourseViewModel>();
        CreateMap<CreateCourseViewModel, CreateCourseDto>();
        CreateMap<EditCourseViewModel, UpdateCourseDto>();

        // Enrollment mappings
        CreateMap<EnrollmentDto, EnrollmentViewModel>();
        CreateMap<EnrollmentDto, EditEnrollmentViewModel>();
        CreateMap<CreateEnrollmentViewModel, CreateEnrollmentDto>();
        CreateMap<EditEnrollmentViewModel, UpdateEnrollmentDto>();
    }
}
