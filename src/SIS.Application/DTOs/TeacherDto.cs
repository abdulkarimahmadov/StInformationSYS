namespace SIS.Application.DTOs;

public class TeacherDto
{
    public int Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime HireDate { get; set; }
    public string? Specialization { get; set; }
    public string Status { get; set; } = "Active";
    public string FullName => $"{FirstName} {LastName}";
}

public class CreateTeacherDto
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
    public string? Specialization { get; set; }
}

public class UpdateTeacherDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public string? Specialization { get; set; }
    public string Status { get; set; } = "Active";
}
