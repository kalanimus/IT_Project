using DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace ApplicationContext;

public class AppDbContext : DbContext
{
  public DbSet<ModelGroup> Groups { get; set;}
  public DbSet<ModelGroupStudent> GroupStudents { get; set; }
  public DbSet<ModelGroupTeacher> GroupTeachers { get; set; }
  public DbSet<ModelPermission> Permissions { get; set; }
  public DbSet<ModelPermissionForRole> PermissionsForRoles { get; set; }
  public DbSet<ModelRole> Roles { get; set; }
  public DbSet<ModelSubject> Subjects { get; set; }
  public DbSet<ModelUser> Users { get; set; }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=KALM_Db;Username=postgres;Password=6257");
  }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи многие-ко-многим для GroupStudents
        modelBuilder.Entity<ModelGroupStudent>()
            .HasKey(gs => new { gs.GroupId, gs.StudentId });

        modelBuilder.Entity<ModelGroupStudent>()
            .HasOne(gs => gs.Group)
            .WithMany(g => g.GroupStudents)
            .HasForeignKey(gs => gs.GroupId);

        modelBuilder.Entity<ModelGroupStudent>()
            .HasOne(gs => gs.Student)
            .WithMany(u => u.GroupStudents)
            .HasForeignKey(gs => gs.StudentId);

        // Настройка связи многие-ко-многим для GroupTeachers
        modelBuilder.Entity<ModelGroupTeacher>()
            .HasKey(gt => new { gt.GroupId, gt.TeacherId, gt.SubjectId });

        modelBuilder.Entity<ModelGroupTeacher>()
            .HasOne(gt => gt.Group)
            .WithMany(g => g.GroupTeachers)
            .HasForeignKey(gt => gt.GroupId);

        modelBuilder.Entity<ModelGroupTeacher>()
            .HasOne(gt => gt.Teacher)
            .WithMany(u => u.GroupTeachers)
            .HasForeignKey(gt => gt.TeacherId);

        modelBuilder.Entity<ModelGroupTeacher>()
            .HasOne(gt => gt.Subject)
            .WithMany(s => s.GroupTeachers)
            .HasForeignKey(gt => gt.SubjectId);

        // Настройка связи многие-ко-многим для PermissionsForRoles
        modelBuilder.Entity<ModelPermissionForRole>()
            .HasKey(pr => new { pr.RoleId, pr.PermissionId });

        modelBuilder.Entity<ModelPermissionForRole>()
            .HasOne(pr => pr.Role)
            .WithMany(r => r.PermissionsForRoles)
            .HasForeignKey(pr => pr.RoleId);

        modelBuilder.Entity<ModelPermissionForRole>()
            .HasOne(pr => pr.Permission)
            .WithMany(p => p.PermissionsForRoles)
            .HasForeignKey(pr => pr.PermissionId);

        // Настройка связи один-ко-многим для Users и Roles
        modelBuilder.Entity<ModelUser>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
    }
}
