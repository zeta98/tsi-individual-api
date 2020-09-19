using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UdelarOnlineApi.Entities
{
  public class UdelarOnlineContext : DbContext
  {
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseUser> CourseUsers { get; set; }

    public UdelarOnlineContext(DbContextOptions<UdelarOnlineContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new FacultyEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
      modelBuilder.ApplyConfiguration(new CourseUserEntityTypeConfiguration());
    }
  }

  public class CourseUserEntityTypeConfiguration : IEntityTypeConfiguration<CourseUser>
  {
    public void Configure(EntityTypeBuilder<CourseUser> courseUserEntityTypeBuilder)
    {
      courseUserEntityTypeBuilder.HasKey(_ => new { _.CourseId, _.UserId });

      courseUserEntityTypeBuilder.HasOne(_ => _.Course).WithMany(_ => _.CourseUsers).HasForeignKey(_ => _.CourseId);
      courseUserEntityTypeBuilder.HasOne(_ => _.User).WithMany(_ => _.CourseUsers).HasForeignKey(_ => _.UserId);
    }
  }

  public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
  {
    public void Configure(EntityTypeBuilder<User> userEntityTypeBuilder)
    {
      userEntityTypeBuilder.HasKey(_ => _.Id);

      userEntityTypeBuilder.HasOne(_ => _.Faculty)
        .WithMany(_ => _.Users)
        .HasForeignKey(_ => _.FacultyId);
    }
  }

  public class FacultyEntityTypeConfiguration : IEntityTypeConfiguration<Faculty>
  {
    public void Configure(EntityTypeBuilder<Faculty> facultyEntityTypeBuilder)
    {
      facultyEntityTypeBuilder.HasKey(_ => _.Id);
    }
  }

  public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
  {
    public void Configure(EntityTypeBuilder<Course> courseEntityTypeBuilder)
    {
      courseEntityTypeBuilder.HasKey(_ => _.Id);

      courseEntityTypeBuilder.HasOne(_ => _.Faculty)
        .WithMany(_ => _.Courses)
        .HasForeignKey(_ => _.FacultyId);
    }
  }
}