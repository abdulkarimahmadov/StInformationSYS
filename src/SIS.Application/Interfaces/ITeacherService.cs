using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
    Task<TeacherDto?> GetTeacherByIdAsync(int id);
    Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto dto);
    Task<TeacherDto> UpdateTeacherAsync(int id, UpdateTeacherDto dto);
    Task<bool> DeleteTeacherAsync(int id);
    Task<IEnumerable<TeacherDto>> GetTeachersByDepartmentAsync(int departmentId);
}
