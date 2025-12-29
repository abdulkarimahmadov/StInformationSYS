using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SIS.Application.DTOs;
using SIS.Application.Exceptions;
using SIS.Application.Interfaces;
using SIS.Domain.Entities;
using SIS.Domain.Enums;

namespace SIS.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EnrollmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync()
    {
        var enrollments = await _unitOfWork.Enrollments
            .GetQueryable()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Where(e => !e.IsDeleted)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EnrollmentDto>>(enrollments);
    }

    public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id)
    {
        var enrollment = await _unitOfWork.Enrollments
            .GetQueryable()
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        return enrollment == null ? null : _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<EnrollmentDto> CreateEnrollmentAsync(CreateEnrollmentDto dto)
    {
        // Check if student exists
        var student = await _unitOfWork.Students.GetByIdAsync(dto.StudentId);
        if (student == null)
        {
            throw new NotFoundException(nameof(Student), dto.StudentId);
        }

        // Check if course exists
        var course = await _unitOfWork.Courses.GetByIdAsync(dto.CourseId);
        if (course == null)
        {
            throw new NotFoundException(nameof(Course), dto.CourseId);
        }

        // Check if already enrolled
        if (await IsStudentEnrolledInCourseAsync(dto.StudentId, dto.CourseId))
        {
            throw new BusinessException("Student is already enrolled in this course.");
        }

        // Check if course is full
        if (await IsCourseFullAsync(dto.CourseId))
        {
            throw new BusinessException("Course has reached maximum capacity.");
        }

        var enrollment = _mapper.Map<Enrollment>(dto);
        await _unitOfWork.Enrollments.AddAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<EnrollmentDto> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto dto)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
        if (enrollment == null)
        {
            throw new NotFoundException(nameof(Enrollment), id);
        }

        enrollment.Status = Enum.Parse<EnrollmentStatus>(dto.Status);
        enrollment.Grade = dto.Grade;
        enrollment.LetterGrade = dto.LetterGrade;

        await _unitOfWork.Enrollments.UpdateAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<EnrollmentDto>(enrollment);
    }

    public async Task<bool> DeleteEnrollmentAsync(int id)
    {
        var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
        if (enrollment == null)
        {
            return false;
        }

        await _unitOfWork.Enrollments.DeleteAsync(enrollment);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId)
    {
        var enrollments = await _unitOfWork.Enrollments
            .FindAsync(e => e.StudentId == studentId && 
                           e.CourseId == courseId && 
                           e.Status == EnrollmentStatus.Enrolled);
        return enrollments.Any();
    }

    public async Task<bool> IsCourseFullAsync(int courseId)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
        if (course == null)
        {
            return true;
        }

        var enrollmentCount = (await _unitOfWork.Enrollments
            .FindAsync(e => e.CourseId == courseId && 
                           e.Status == EnrollmentStatus.Enrolled)).Count();
        return enrollmentCount >= course.MaxCapacity;
    }
}
