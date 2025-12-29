using SIS.Domain.Common;
using SIS.Domain.Enums;

namespace SIS.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Enrolled;
    public decimal? Grade { get; set; }
    public string? LetterGrade { get; set; }
    public decimal? AttendancePercentage { get; set; }

    // Navigation properties
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
}
