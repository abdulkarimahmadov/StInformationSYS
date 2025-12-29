using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendance");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(a => a.Notes)
            .HasMaxLength(200);
    }
}
