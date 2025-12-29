using System.ComponentModel.DataAnnotations;

namespace SIS.Web.ViewModels;

public class CourseViewModel
{
    public int Id { get; set; }

    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;

    [Display(Name = "Course Name")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Credits")]
    public int Credits { get; set; }

    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    [Display(Name = "Department Name")]
    public string? DepartmentName { get; set; }

    [Display(Name = "Teacher")]
    public int? TeacherId { get; set; }

    [Display(Name = "Teacher Name")]
    public string? TeacherName { get; set; }

    [Display(Name = "Max Capacity")]
    public int MaxCapacity { get; set; }

    [Display(Name = "Current Enrollment")]
    public int CurrentEnrollment { get; set; }

    [Display(Name = "Semester")]
    public int SemesterId { get; set; }

    [Display(Name = "Semester Name")]
    public string? SemesterName { get; set; }

    [Display(Name = "Schedule")]
    public string? Schedule { get; set; }

    [Display(Name = "Room")]
    public string? Room { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Active";

    [Display(Name = "Available Seats")]
    public int AvailableSeats => MaxCapacity - CurrentEnrollment;
}

public class CreateCourseViewModel
{
    [Required(ErrorMessage = "Course code is required")]
    [Display(Name = "Course Code")]
    [StringLength(20, ErrorMessage = "Course code cannot exceed 20 characters")]
    public string CourseCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course name is required")]
    [Display(Name = "Course Name")]
    [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Credits are required")]
    [Display(Name = "Credits")]
    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
    public int Credits { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    [Display(Name = "Teacher")]
    public int? TeacherId { get; set; }

    [Required(ErrorMessage = "Max capacity is required")]
    [Display(Name = "Max Capacity")]
    [Range(1, 500, ErrorMessage = "Max capacity must be between 1 and 500")]
    public int MaxCapacity { get; set; }

    [Required(ErrorMessage = "Semester is required")]
    [Display(Name = "Semester")]
    public int SemesterId { get; set; }

    [Display(Name = "Schedule")]
    [StringLength(100, ErrorMessage = "Schedule cannot exceed 100 characters")]
    public string? Schedule { get; set; }

    [Display(Name = "Room")]
    [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
    public string? Room { get; set; }
}

public class EditCourseViewModel
{
    public int Id { get; set; }

    [Display(Name = "Course Code")]
    public string CourseCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Course name is required")]
    [Display(Name = "Course Name")]
    [StringLength(100, ErrorMessage = "Course name cannot exceed 100 characters")]
    public string CourseName { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Credits are required")]
    [Display(Name = "Credits")]
    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
    public int Credits { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }

    [Display(Name = "Teacher")]
    public int? TeacherId { get; set; }

    [Required(ErrorMessage = "Max capacity is required")]
    [Display(Name = "Max Capacity")]
    [Range(1, 500, ErrorMessage = "Max capacity must be between 1 and 500")]
    public int MaxCapacity { get; set; }

    [Required(ErrorMessage = "Semester is required")]
    [Display(Name = "Semester")]
    public int SemesterId { get; set; }

    [Display(Name = "Schedule")]
    [StringLength(100, ErrorMessage = "Schedule cannot exceed 100 characters")]
    public string? Schedule { get; set; }

    [Display(Name = "Room")]
    [StringLength(50, ErrorMessage = "Room cannot exceed 50 characters")]
    public string? Room { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Active";
}
