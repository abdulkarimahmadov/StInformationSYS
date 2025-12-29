using SIS.Domain.Common;
using SIS.Domain.Enums;

namespace SIS.Domain.Entities;

public class Student : BaseEntity
{
    public string StudentNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public int GradeLevel { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;

    // Navigation properties
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public string FullName => $"{FirstName} {LastName}";
}
