using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(e => e.Grade)
            .HasPrecision(5, 2);

        builder.Property(e => e.LetterGrade)
            .HasMaxLength(2);

        builder.Property(e => e.AttendancePercentage)
            .HasPrecision(5, 2);

        // Unique constraint for student-course pair
        builder.HasIndex(e => new { e.StudentId, e.CourseId })
            .IsUnique();

        // Relationships
        builder.HasMany(e => e.Grades)
            .WithOne(g => g.Enrollment)
            .HasForeignKey(g => g.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.AttendanceRecords)
            .WithOne(a => a.Enrollment)
            .HasForeignKey(a => a.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
