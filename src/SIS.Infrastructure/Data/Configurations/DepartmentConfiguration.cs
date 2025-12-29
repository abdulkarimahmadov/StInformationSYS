using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DepartmentCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(d => d.DepartmentCode)
            .IsUnique();

        builder.Property(d => d.DepartmentName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);
    }
}
