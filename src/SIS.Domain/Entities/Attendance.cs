using SIS.Domain.Common;
using SIS.Domain.Enums;

namespace SIS.Domain.Entities;

public class Attendance : BaseEntity
{
    public int EnrollmentId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public Enrollment Enrollment { get; set; } = null!;
}
