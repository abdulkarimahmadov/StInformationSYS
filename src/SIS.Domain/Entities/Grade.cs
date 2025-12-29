using SIS.Domain.Common;

namespace SIS.Domain.Entities;

public class Grade : BaseEntity
{
    public int EnrollmentId { get; set; }
    public string AssignmentName { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal? Weight { get; set; }
    public DateTime GradeDate { get; set; }
    public string? Comments { get; set; }

    // Navigation properties
    public Enrollment Enrollment { get; set; } = null!;
}
