namespace SIS.Application.DTOs;

public class SemesterDto
{
    public int Id { get; set; }
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentSemester { get; set; }
    public int CourseCount { get; set; }
}

public class CreateSemesterDto
{
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentSemester { get; set; }
}

public class UpdateSemesterDto
{
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentSemester { get; set; }
}
