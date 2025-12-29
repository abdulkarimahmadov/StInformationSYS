using SIS.Application.Interfaces;
using SIS.Domain.Entities;
using SIS.Infrastructure.Data;

namespace SIS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IGenericRepository<Student>? _students;
    private IGenericRepository<Course>? _courses;
    private IGenericRepository<Enrollment>? _enrollments;
    private IGenericRepository<Grade>? _grades;
    private IGenericRepository<Teacher>? _teachers;
    private IGenericRepository<Department>? _departments;
    private IGenericRepository<Semester>? _semesters;
    private IGenericRepository<Attendance>? _attendances;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Student> Students =>
        _students ??= new GenericRepository<Student>(_context);

    public IGenericRepository<Course> Courses =>
        _courses ??= new GenericRepository<Course>(_context);

    public IGenericRepository<Enrollment> Enrollments =>
        _enrollments ??= new GenericRepository<Enrollment>(_context);

    public IGenericRepository<Grade> Grades =>
        _grades ??= new GenericRepository<Grade>(_context);

    public IGenericRepository<Teacher> Teachers =>
        _teachers ??= new GenericRepository<Teacher>(_context);

    public IGenericRepository<Department> Departments =>
        _departments ??= new GenericRepository<Department>(_context);

    public IGenericRepository<Semester> Semesters =>
        _semesters ??= new GenericRepository<Semester>(_context);

    public IGenericRepository<Attendance> Attendances =>
        _attendances ??= new GenericRepository<Attendance>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
