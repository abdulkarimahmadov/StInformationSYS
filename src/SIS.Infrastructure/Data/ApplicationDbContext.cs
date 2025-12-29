using Microsoft.EntityFrameworkCore;
using SIS.Domain.Common;
using SIS.Domain.Entities;

namespace SIS.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Semester> Semesters { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Global query filter for soft delete
        modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Course>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Enrollment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Grade>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Teacher>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Semester>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Attendance>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                   (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }

            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
