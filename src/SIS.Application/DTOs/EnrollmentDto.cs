namespace SIS.Application.DTOs;

public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public string? StudentNumber { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public string? CourseCode { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public string Status { get; set; } = "Enrolled";
    public decimal? Grade { get; set; }
    public string? LetterGrade { get; set; }
    public decimal? AttendancePercentage { get; set; }
}

public class CreateEnrollmentDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}

public class UpdateEnrollmentDto
{
    public string Status { get; set; } = "Enrolled";
    public decimal? Grade { get; set; }
    public string? LetterGrade { get; set; }
}
