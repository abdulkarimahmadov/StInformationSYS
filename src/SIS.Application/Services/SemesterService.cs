using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class SemesterService : ISemesterService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SemesterService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SemesterDto>> GetAllSemestersAsync()
    {
        var semesters = await _unitOfWork.Semesters
            .GetQueryable()
            .Include(s => s.Courses)
            .Where(s => !s.IsDeleted)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync();
        return _mapper.Map<IEnumerable<SemesterDto>>(semesters);
    }

    public async Task<SemesterDto?> GetSemesterByIdAsync(int id)
    {
        var semester = await _unitOfWork.Semesters
            .GetQueryable()
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        return semester == null ? null : _mapper.Map<SemesterDto>(semester);
    }

    public async Task<SemesterDto> CreateSemesterAsync(CreateSemesterDto dto)
    {
        var semester = _mapper.Map<Semester>(dto);

        // If this is set as current semester, unset all others
        if (dto.IsCurrentSemester)
        {
            await UnsetAllCurrentSemestersAsync();
        }

        await _unitOfWork.Semesters.AddAsync(semester);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SemesterDto>(semester);
    }

    public async Task<SemesterDto> UpdateSemesterAsync(int id, UpdateSemesterDto dto)
    {
        var semester = await _unitOfWork.Semesters.GetByIdAsync(id);
        if (semester == null)
        {
            throw new NotFoundException(nameof(Semester), id);
        }

        // If this is set as current semester, unset all others
        if (dto.IsCurrentSemester && !semester.IsCurrentSemester)
        {
            await UnsetAllCurrentSemestersAsync();
        }

        _mapper.Map(dto, semester);
        await _unitOfWork.Semesters.UpdateAsync(semester);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SemesterDto>(semester);
    }

    public async Task<bool> DeleteSemesterAsync(int id)
    {
        var semester = await _unitOfWork.Semesters.GetByIdAsync(id);
        if (semester == null)
        {
            return false;
        }

        await _unitOfWork.Semesters.DeleteAsync(semester);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<SemesterDto?> GetCurrentSemesterAsync()
    {
        var semester = await _unitOfWork.Semesters
            .GetQueryable()
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.IsCurrentSemester && !s.IsDeleted);
        return semester == null ? null : _mapper.Map<SemesterDto>(semester);
    }

    private async Task UnsetAllCurrentSemestersAsync()
    {
        var currentSemesters = await _unitOfWork.Semesters
            .FindAsync(s => s.IsCurrentSemester);
        
        foreach (var semester in currentSemesters)
        {
            semester.IsCurrentSemester = false;
            await _unitOfWork.Semesters.UpdateAsync(semester);
        }
    }
}
