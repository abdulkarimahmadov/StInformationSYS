using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators;

public class CreateEnrollmentValidator : AbstractValidator<CreateEnrollmentDto>
{
    public CreateEnrollmentValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student is required");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course is required");
    }
}

public class UpdateEnrollmentValidator : AbstractValidator<UpdateEnrollmentDto>
{
    public UpdateEnrollmentValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(BeValidStatus).WithMessage("Invalid status value");

        RuleFor(x => x.Grade)
            .InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100")
            .When(x => x.Grade.HasValue);

        RuleFor(x => x.LetterGrade)
            .MaximumLength(2).WithMessage("Letter grade cannot exceed 2 characters")
            .When(x => !string.IsNullOrEmpty(x.LetterGrade));
    }

    private bool BeValidStatus(string status)
    {
        return Enum.TryParse<Domain.Enums.EnrollmentStatus>(status, out _);
    }
}
