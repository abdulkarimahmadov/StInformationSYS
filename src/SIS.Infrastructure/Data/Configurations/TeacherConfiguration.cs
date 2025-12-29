using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(t => t.EmployeeNumber)
            .IsUnique();

        builder.Property(t => t.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(t => t.Email)
            .IsUnique();

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(t => t.Specialization)
            .HasMaxLength(100);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationships
        builder.HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
