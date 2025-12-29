using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    Task<CourseDto?> GetCourseByIdAsync(int id);
    Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);
    Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteCourseAsync(int id);
    Task<IEnumerable<CourseDto>> SearchCoursesAsync(string searchTerm);
    Task<IEnumerable<CourseDto>> GetCoursesByDepartmentAsync(int departmentId);
    Task<IEnumerable<CourseDto>> GetCoursesBySemesterAsync(int semesterId);
    Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(int courseId);
}
