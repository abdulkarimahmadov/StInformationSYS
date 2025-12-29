using AutoMapper;
using SIS.Application.DTOs;
using SIS.Domain.Entities;

namespace SIS.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Student mappings
        CreateMap<Student, StudentDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateStudentDto, Student>()
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.StudentStatus.Active));
        CreateMap<UpdateStudentDto, Student>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Domain.Enums.StudentStatus>(src.Status)));

        // Course mappings
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : null))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.FullName : null))
            .ForMember(dest => dest.SemesterName, opt => opt.MapFrom(src => src.Semester != null ? src.Semester.SemesterName : null))
            .ForMember(dest => dest.CurrentEnrollment, opt => opt.MapFrom(src => src.Enrollments.Count));
        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.CourseStatus.Active));
        CreateMap<UpdateCourseDto, Course>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Domain.Enums.CourseStatus>(src.Status)));

        // Enrollment mappings
        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student != null ? src.Student.FullName : null))
            .ForMember(dest => dest.StudentNumber, opt => opt.MapFrom(src => src.Student != null ? src.Student.StudentNumber : null))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseName : null))
            .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course != null ? src.Course.CourseCode : null));
        CreateMap<CreateEnrollmentDto, Enrollment>()
            .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.EnrollmentStatus.Enrolled));

        // Grade mappings
        CreateMap<Grade, GradeDto>();
        CreateMap<CreateGradeDto, Grade>();
        CreateMap<UpdateGradeDto, Grade>();

        // Department mappings
        CreateMap<Department, DepartmentDto>()
            .ForMember(dest => dest.TeacherCount, opt => opt.MapFrom(src => src.Teachers.Count))
            .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count));
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Teacher mappings
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.DepartmentName : null));
        CreateMap<CreateTeacherDto, Teacher>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.TeacherStatus.Active));
        CreateMap<UpdateTeacherDto, Teacher>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<Domain.Enums.TeacherStatus>(src.Status)));

        // Semester mappings
        CreateMap<Semester, SemesterDto>()
            .ForMember(dest => dest.CourseCount, opt => opt.MapFrom(src => src.Courses.Count));
        CreateMap<CreateSemesterDto, Semester>();
        CreateMap<UpdateSemesterDto, Semester>();
    }
}
