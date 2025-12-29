using SIS.Domain.Common;
using SIS.Domain.Enums;

namespace SIS.Domain.Entities;

public class Teacher : BaseEntity
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
    public string? Specialization { get; set; }
    public TeacherStatus Status { get; set; } = TeacherStatus.Active;

    // Navigation properties
    public Department Department { get; set; } = null!;
    public ICollection<Course> Courses { get; set; } = new List<Course>();

    public string FullName => $"{FirstName} {LastName}";
}
