using SIS.Domain.Common;
using SIS.Domain.Enums;

namespace SIS.Domain.Entities;

public class Course : BaseEntity
{
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public int? TeacherId { get; set; }
    public int MaxCapacity { get; set; }
    public int SemesterId { get; set; }
    public string? Schedule { get; set; }
    public string? Room { get; set; }
    public CourseStatus Status { get; set; } = CourseStatus.Active;

    // Navigation properties
    public Department Department { get; set; } = null!;
    public Teacher? Teacher { get; set; }
    public Semester Semester { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
