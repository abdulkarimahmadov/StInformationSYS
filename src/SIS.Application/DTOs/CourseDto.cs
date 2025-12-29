namespace SIS.Application.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string CourseCode { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public int MaxCapacity { get; set; }
    public int CurrentEnrollment { get; set; }
    public int SemesterId { get; set; }
    public string? SemesterName { get; set; }
    public string? Schedule { get; set; }
    public string? Room { get; set; }
    public string Status { get; set; } = "Active";
}

public class CreateCourseDto
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
}

public class UpdateCourseDto
{
    public string CourseName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
    public int? TeacherId { get; set; }
    public int MaxCapacity { get; set; }
    public int SemesterId { get; set; }
    public string? Schedule { get; set; }
    public string? Room { get; set; }
    public string Status { get; set; } = "Active";
}
