using System.ComponentModel.DataAnnotations;

namespace SIS.Web.ViewModels;

public class EnrollmentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Display(Name = "Student Name")]
    public string? StudentName { get; set; }

    [Display(Name = "Student Number")]
    public string? StudentNumber { get; set; }

    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Display(Name = "Course Name")]
    public string? CourseName { get; set; }

    [Display(Name = "Course Code")]
    public string? CourseCode { get; set; }

    [Display(Name = "Enrollment Date")]
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Enrolled";

    [Display(Name = "Grade")]
    public decimal? Grade { get; set; }

    [Display(Name = "Letter Grade")]
    public string? LetterGrade { get; set; }

    [Display(Name = "Attendance")]
    public decimal? AttendancePercentage { get; set; }
}

public class CreateEnrollmentViewModel
{
    [Required(ErrorMessage = "Student is required")]
    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Course is required")]
    [Display(Name = "Course")]
    public int CourseId { get; set; }
}

public class EditEnrollmentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Student")]
    public int StudentId { get; set; }

    [Display(Name = "Student Name")]
    public string? StudentName { get; set; }

    [Display(Name = "Course")]
    public int CourseId { get; set; }

    [Display(Name = "Course Name")]
    public string? CourseName { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Enrolled";

    [Display(Name = "Grade")]
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
    public decimal? Grade { get; set; }

    [Display(Name = "Letter Grade")]
    [StringLength(2, ErrorMessage = "Letter grade cannot exceed 2 characters")]
    public string? LetterGrade { get; set; }
}
