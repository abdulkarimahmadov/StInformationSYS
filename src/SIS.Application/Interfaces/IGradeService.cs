using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface IGradeService
{
    Task<IEnumerable<GradeDto>> GetGradesByEnrollmentAsync(int enrollmentId);
    Task<GradeDto?> GetGradeByIdAsync(int id);
    Task<GradeDto> CreateGradeAsync(CreateGradeDto dto);
    Task<GradeDto> UpdateGradeAsync(int id, UpdateGradeDto dto);
    Task<bool> DeleteGradeAsync(int id);
    Task<decimal> CalculateAverageGradeAsync(int enrollmentId);
    Task<string> CalculateLetterGradeAsync(decimal percentage);
}
