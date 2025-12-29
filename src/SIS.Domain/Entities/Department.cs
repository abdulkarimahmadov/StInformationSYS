using SIS.Domain.Common;

namespace SIS.Domain.Entities;

public class Department : BaseEntity
{
    public string DepartmentCode { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? HeadOfDepartment { get; set; }

    // Navigation properties
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
