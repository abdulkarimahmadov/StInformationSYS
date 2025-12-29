using SIS.Domain.Entities;

namespace SIS.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Student> Students { get; }
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Enrollment> Enrollments { get; }
    IGenericRepository<Grade> Grades { get; }
    IGenericRepository<Teacher> Teachers { get; }
    IGenericRepository<Department> Departments { get; }
    IGenericRepository<Semester> Semesters { get; }
    IGenericRepository<Attendance> Attendances { get; }
    Task<int> SaveChangesAsync();
}
