using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.ToTable("Grades");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.AssignmentName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Score)
            .HasPrecision(5, 2);

        builder.Property(g => g.MaxScore)
            .HasPrecision(5, 2);

        builder.Property(g => g.Weight)
            .HasPrecision(5, 2);

        builder.Property(g => g.Comments)
            .HasMaxLength(500);
    }
}
