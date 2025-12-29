namespace SIS.Application.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string DepartmentCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? HeadOfDepartment { get; set; }
    public int TeacherCount { get; set; }
    public int CourseCount { get; set; }
}

public class CreateDepartmentDto
{
    public string DepartmentCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? HeadOfDepartment { get; set; }
}

public class UpdateDepartmentDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? HeadOfDepartment { get; set; }
}
