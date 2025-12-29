using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _unitOfWork.Students.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        return student == null ? null : _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentDto dto)
    {
        // Check for duplicate student number
        var existingStudents = await _unitOfWork.Students
            .FindAsync(s => s.StudentNumber == dto.StudentNumber);
        if (existingStudents.Any())
        {
            throw new BusinessException($"A student with number {dto.StudentNumber} already exists.");
        }

        // Check for duplicate email
        existingStudents = await _unitOfWork.Students
            .FindAsync(s => s.Email == dto.Email);
        if (existingStudents.Any())
        {
            throw new BusinessException($"A student with email {dto.Email} already exists.");
        }

        var student = _mapper.Map<Student>(dto);
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> UpdateStudentAsync(int id, UpdateStudentDto dto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
        {
            throw new NotFoundException(nameof(Student), id);
        }

        // Check for duplicate email (excluding current student)
        var existingStudents = await _unitOfWork.Students
            .FindAsync(s => s.Email == dto.Email && s.Id != id);
        if (existingStudents.Any())
        {
            throw new BusinessException($"A student with email {dto.Email} already exists.");
        }

        _mapper.Map(dto, student);
        await _unitOfWork.Students.UpdateAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
        {
            return false;
        }

        await _unitOfWork.Students.DeleteAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StudentDto>> SearchStudentsAsync(string searchTerm)
    {
        var students = await _unitOfWork.Students.FindAsync(s =>
            s.FirstName.Contains(searchTerm) ||
            s.LastName.Contains(searchTerm) ||
            s.StudentNumber.Contains(searchTerm) ||
            s.Email.Contains(searchTerm));
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetStudentEnrollmentsAsync(int studentId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .GetQueryable()
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId && !e.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }
}
