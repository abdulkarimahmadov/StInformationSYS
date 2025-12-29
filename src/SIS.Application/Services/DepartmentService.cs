using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
    {
        var departments = await _unitOfWork.Departments
            .GetQueryable()
            .Include(d => d.Teachers)
            .Include(d => d.Courses)
            .Where(d => !d.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
    {
        var department = await _unitOfWork.Departments
            .GetQueryable()
            .Include(d => d.Teachers)
            .Include(d => d.Courses)
            .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
        return department == null ? null : _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        // Check for duplicate department code
        var existingDepartments = await _unitOfWork.Departments
            .FindAsync(d => d.DepartmentCode == dto.DepartmentCode);
        if (existingDepartments.Any())
        {
            throw new BusinessException($"A department with code {dto.DepartmentCode} already exists.");
        }

        var department = _mapper.Map<Department>(dto);
        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<DepartmentDto> UpdateDepartmentAsync(int id, UpdateDepartmentDto dto)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            throw new NotFoundException(nameof(Department), id);
        }

        _mapper.Map(dto, department);
        await _unitOfWork.Departments.UpdateAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DepartmentDto>(department);
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            return false;
        }

        await _unitOfWork.Departments.DeleteAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
