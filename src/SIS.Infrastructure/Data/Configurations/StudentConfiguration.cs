using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StudentNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(s => s.StudentNumber)
            .IsUnique();

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(s => s.Email)
            .IsUnique();

        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(s => s.Gender)
            .HasMaxLength(10);

        builder.Property(s => s.Address)
            .HasMaxLength(200);

        builder.Property(s => s.City)
            .HasMaxLength(50);

        builder.Property(s => s.Country)
            .HasMaxLength(50);

        builder.Property(s => s.PostalCode)
            .HasMaxLength(10);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationships
        builder.HasMany(s => s.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
