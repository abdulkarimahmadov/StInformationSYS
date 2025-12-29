using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators;

public class CreateCourseValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseValidator()
    {
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("Course code is required")
            .MaximumLength(20).WithMessage("Course code cannot exceed 20 characters");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course name is required")
            .MaximumLength(100).WithMessage("Course name cannot exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 10).WithMessage("Credits must be between 1 and 10");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department is required");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

        RuleFor(x => x.SemesterId)
            .GreaterThan(0).WithMessage("Semester is required");

        RuleFor(x => x.Schedule)
            .MaximumLength(100).WithMessage("Schedule cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Schedule));

        RuleFor(x => x.Room)
            .MaximumLength(50).WithMessage("Room cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.Room));
    }
}

public class UpdateCourseValidator : AbstractValidator<UpdateCourseDto>
{
    public UpdateCourseValidator()
    {
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course name is required")
            .MaximumLength(100).WithMessage("Course name cannot exceed 100 characters");

        RuleFor(x => x.Credits)
            .InclusiveBetween(1, 10).WithMessage("Credits must be between 1 and 10");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department is required");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Max capacity must be greater than 0");

        RuleFor(x => x.SemesterId)
            .GreaterThan(0).WithMessage("Semester is required");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status value");
    }

    private bool BeValidStatus(string status)
    {
        return Enum.TryParse<Domain.Enums.CourseStatus>(status, out _);
    }
}
