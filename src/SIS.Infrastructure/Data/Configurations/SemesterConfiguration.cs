using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class SemesterConfiguration : IEntityTypeConfiguration<Semester>
{
    public void Configure(EntityTypeBuilder<Semester> builder)
    {
        builder.ToTable("Semesters");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SemesterName)
            .IsRequired()
            .HasMaxLength(50);
    }
}
