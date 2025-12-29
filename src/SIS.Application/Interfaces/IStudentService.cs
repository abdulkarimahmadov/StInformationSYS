using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<StudentDto> CreateStudentAsync(CreateStudentDto dto);
    Task<StudentDto> UpdateStudentAsync(int id, UpdateStudentDto dto);
    Task<bool> DeleteStudentAsync(int id);
    Task<IEnumerable<StudentDto>> SearchStudentsAsync(string searchTerm);
    Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(int studentId);
}
