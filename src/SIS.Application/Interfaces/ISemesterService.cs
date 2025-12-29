using SIS.Application.DTOs;

namespace SIS.Application.Interfaces;

public interface ISemesterService
{
    Task<IEnumerable<SemesterDto>> GetAllSemestersAsync();
    Task<SemesterDto?> GetSemesterByIdAsync(int id);
    Task<SemesterDto> CreateSemesterAsync(CreateSemesterDto dto);
    Task<SemesterDto> UpdateSemesterAsync(int id, UpdateSemesterDto dto);
    Task<bool> DeleteSemesterAsync(int id);
    Task<SemesterDto?> GetCurrentSemesterAsync();
}
