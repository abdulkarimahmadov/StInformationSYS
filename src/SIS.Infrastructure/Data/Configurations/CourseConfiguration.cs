using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CourseCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(c => c.CourseCode)
            .IsUnique();

        builder.Property(c => c.CourseName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Schedule)
            .HasMaxLength(100);

        builder.Property(c => c.Room)
            .HasMaxLength(50);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Teacher)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(c => c.Semester)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SemesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
