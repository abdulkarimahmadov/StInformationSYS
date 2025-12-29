using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync();
    Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);
    Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto dto);
    Task<EnrollmentDto> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteEnrollmentAsync(int id);
    Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId);
    Task<bool> IsCourseFullAsync(int courseId);
}
