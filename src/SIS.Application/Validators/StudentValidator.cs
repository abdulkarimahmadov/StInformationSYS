using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators;

public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.StudentNumber)
            .NotEmpty().WithMessage("Student number is required")
            .MaximumLength(20).WithMessage("Student number cannot exceed 20 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(BeAtLeast5YearsOld).WithMessage("Student must be at least 5 years old");

        RuleFor(x => x.GradeLevel)
            .InclusiveBetween(1, 12).WithMessage("Grade level must be between 1 and 12");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }

    private bool BeAtLeast5YearsOld(DateTime dateOfBirth)
    {
        return dateOfBirth <= DateTime.UtcNow.AddYears(-5);
    }
}

public class UpdateStudentValidator : AbstractValidator<UpdateStudentDto>
{
    public UpdateStudentValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.GradeLevel)
            .InclusiveBetween(1, 12).WithMessage("Grade level must be between 1 and 12");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status value");
    }

    private bool BeValidStatus(string status)
    {
        return Enum.TryParse<Domain.Enums.StudentStatus>(status, out _);
    }
}
