using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class GradeService : IGradeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GradeService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GradeDto>> GetGradesByEnrollmentAsync(int enrollmentId)
    {
        var grades = await _unitOfWork.Grades
            .FindAsync(g => g.EnrollmentId == enrollmentId);
        return _mapper.Map<IEnumerable<GradeDto>>(grades);
    }

    public async Task<GradeDto?> GetGradeByIdAsync(int id)
    {
        var grade = await _unitOfWork.Grades.GetByIdAsync(id);
        return grade == null ? null : _mapper.Map<GradeDto>(grade);
    }

    public async Task<GradeDto> CreateGradeAsync(CreateGradeDto dto)
    {
        // Check if enrollment exists
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(dto.EnrollmentId);
        if (enrollment == null)
        {
            throw new NotFoundException(nameof(Enrollment), dto.EnrollmentId);
        }

        var grade = _mapper.Map<Grade>(dto);
        await _unitOfWork.Grades.AddAsync(grade);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GradeDto>(grade);
    }

    public async Task<GradeDto> UpdateGradeAsync(int id, UpdateGradeDto dto)
    {
        var grade = await _unitOfWork.Grades.GetByIdAsync(id);
        if (grade == null)
        {
            throw new NotFoundException(nameof(Grade), id);
        }

        _mapper.Map(dto, grade);
        await _unitOfWork.Grades.UpdateAsync(grade);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GradeDto>(grade);
    }

    public async Task<bool> DeleteGradeAsync(int id)
    {
        var grade = await _unitOfWork.Grades.GetByIdAsync(id);
        if (grade == null)
        {
            return false;
        }

        await _unitOfWork.Grades.DeleteAsync(grade);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> CalculateAverageGradeAsync(int enrollmentId)
    {
        var grades = await _unitOfWork.Grades
            .FindAsync(g => g.EnrollmentId == enrollmentId);

        if (!grades.Any())
        {
            return 0;
        }

        var gradesWithWeight = grades.Where(g => g.Weight.HasValue && g.Weight.Value > 0);
        if (gradesWithWeight.Any())
        {
            var weightedSum = gradesWithWeight.Sum(g => (g.Score / g.MaxScore) * 100 * g.Weight!.Value);
            var totalWeight = gradesWithWeight.Sum(g => g.Weight!.Value);
            return totalWeight > 0 ? weightedSum / totalWeight : 0;
        }
        else
        {
            return grades.Average(g => (g.Score / g.MaxScore) * 100);
        }
    }

    public Task<string> CalculateLetterGradeAsync(decimal percentage)
    {
        var letterGrade = percentage switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            >= 60 => "D",
            _ => "F"
        };
        return Task.FromResult(letterGrade);
    }
}
