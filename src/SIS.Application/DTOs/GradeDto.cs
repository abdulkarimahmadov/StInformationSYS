namespace SIS.Application.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public string AssignmentName { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal? Weight { get; set; }
    public DateTime GradeDate { get; set; }
    public string? Comments { get; set; }
    public decimal Percentage => MaxScore > 0 ? (Score / MaxScore) * 100 : 0;
}

public class CreateGradeDto
{
    public int EnrollmentId { get; set; }
    public string AssignmentName { get; set; } = string.Empty;
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal? Weight { get; set; }
    public DateTime GradeDate { get; set; }
    public string? Comments { get; set; }
}

public class UpdateGradeDto
{
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal? Weight { get; set; }
    public string? Comments { get; set; }
}
