using SIS.Domain.Common;

namespace SIS.Domain.Entities;

public class Semester : BaseEntity
{
    public string SemesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentSemester { get; set; } = false;

    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
