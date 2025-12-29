using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class TeacherService : ITeacherService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
    {
        var teachers = await _unitOfWork.Teachers
            .GetQueryable()
            .Include(t => t.Department)
            .Where(t => !t.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
    }

    public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers
            .GetQueryable()
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        return teacher == null ? null : _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto dto)
    {
        // Check for duplicate employee number
        var existingTeachers = await _unitOfWork.Teachers
            .FindAsync(t => t.EmployeeNumber == dto.EmployeeNumber);
        if (existingTeachers.Any())
        {
            throw new BusinessException($"A teacher with employee number {dto.EmployeeNumber} already exists.");
        }

        // Check for duplicate email
        existingTeachers = await _unitOfWork.Teachers
            .FindAsync(t => t.Email == dto.Email);
        if (existingTeachers.Any())
        {
            throw new BusinessException($"A teacher with email {dto.Email} already exists.");
        }

        var teacher = _mapper.Map<Teacher>(dto);
        await _unitOfWork.Teachers.AddAsync(teacher);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> UpdateTeacherAsync(int id, UpdateTeacherDto dto)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null)
        {
            throw new NotFoundException(nameof(Teacher), id);
        }

        // Check for duplicate email (excluding current teacher)
        var existingTeachers = await _unitOfWork.Teachers
            .FindAsync(t => t.Email == dto.Email && t.Id != id);
        if (existingTeachers.Any())
        {
            throw new BusinessException($"A teacher with email {dto.Email} already exists.");
        }

        _mapper.Map(dto, teacher);
        await _unitOfWork.Teachers.UpdateAsync(teacher);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null)
        {
            return false;
        }

        await _unitOfWork.Teachers.DeleteAsync(teacher);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersByDepartmentAsync(int departmentId)
    {
        var teachers = await _unitOfWork.Teachers
            .GetQueryable()
            .Include(t => t.Department)
            .Where(t => t.DepartmentId == departmentId && !t.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
    }
}
