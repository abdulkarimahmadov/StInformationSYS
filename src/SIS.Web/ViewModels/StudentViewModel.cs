using System.ComponentModel.DataAnnotations;

namespace SIS.Web.ViewModels;

public class StudentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Student Number")]
    public string StudentNumber { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Address")]
    public string? Address { get; set; }

    [Display(Name = "City")]
    public string? City { get; set; }

    [Display(Name = "Country")]
    public string? Country { get; set; }

    [Display(Name = "Postal Code")]
    public string? PostalCode { get; set; }

    [Display(Name = "Enrollment Date")]
    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }

    [Display(Name = "Grade Level")]
    public int GradeLevel { get; set; }

    [Display(Name = "Status")]
    public string Status { get; set; } = "Active";

    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";
}

public class CreateStudentViewModel
{
    [Required(ErrorMessage = "Student number is required")]
    [Display(Name = "Student Number")]
    [StringLength(20, ErrorMessage = "Student number cannot exceed 20 characters")]
    public string StudentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Address")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string? Address { get; set; }

    [Display(Name = "City")]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
    public string? City { get; set; }

    [Display(Name = "Country")]
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string? Country { get; set; }

    [Display(Name = "Postal Code")]
    [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = "Grade level is required")]
    [Display(Name = "Grade Level")]
    [Range(1, 12, ErrorMessage = "Grade level must be between 1 and 12")]
    public int GradeLevel { get; set; }
}

public class EditStudentViewModel
{
    public int Id { get; set; }

    [Display(Name = "Student Number")]
    public string StudentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [Display(Name = "First Name")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [Display(Name = "Last Name")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone Number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Address")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string? Address { get; set; }

    [Display(Name = "City")]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
    public string? City { get; set; }

    [Display(Name = "Country")]
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string? Country { get; set; }

    [Display(Name = "Postal Code")]
    [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
    public string? PostalCode { get; set; }

    [Required(ErrorMessage = "Grade level is required")]
    [Display(Name = "Grade Level")]
    [Range(1, 12, ErrorMessage = "Grade level must be between 1 and 12")]
    public int GradeLevel { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [Display(Name = "Status")]
    public string Status { get; set; } = "Active";
}
