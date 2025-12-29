using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;

namespace SIS.Application.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _unitOfWork.Courses
            .GetQueryable()
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .Include(c => c.Enrollments)
            .Where(c => !c.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses
            .GetQueryable()
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        return course == null ? null : _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
    {
        // Check for duplicate course code
        var existingCourses = await _unitOfWork.Courses
            .FindAsync(c => c.CourseCode == dto.CourseCode);
        if (existingCourses.Any())
        {
            throw new BusinessException($"A course with code {dto.CourseCode} already exists.");
        }

        var course = _mapper.Map<Course>(dto);
        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            throw new NotFoundException(nameof(Course), id);
        }

        _mapper.Map(dto, course);
        await _unitOfWork.Courses.UpdateAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
        {
            return false;
        }

        await _unitOfWork.Courses.DeleteAsync(course);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CourseDto>> SearchCoursesAsync(string searchTerm)
    {
        var courses = await _unitOfWork.Courses
            .GetQueryable()
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .Where(c => !c.IsDeleted &&
                (c.CourseName.Contains(searchTerm) ||
                c.CourseCode.Contains(searchTerm) ||
                (c.Description != null && c.Description.Contains(searchTerm))))
            .ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByDepartmentAsync(int departmentId)
    {
        var courses = await _unitOfWork.Courses
            .GetQueryable()
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .Where(c => c.DepartmentId == departmentId && !c.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesBySemesterAsync(int semesterId)
    {
        var courses = await _unitOfWork.Courses
            .GetQueryable()
            .Include(c => c.Department)
            .Include(c => c.Teacher)
            .Include(c => c.Semester)
            .Where(c => c.SemesterId == semesterId && !c.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(int courseId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .GetQueryable()
            .Include(e => e.Student)
            .Where(e => e.CourseId == courseId && !e.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }
}
