using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>
/// Контекст Entity Framework Core: описывает, как класс Student отображается на таблицу в базе.
/// </summary>
public class Context : DbContext
{
    /// <summary>
    /// Набор студентов (таблица Students).
    /// </summary>
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentGrade> StudentGrades => Set<StudentGrade>();

    /// <summary>
    /// Настраивает подключение: используется SQL Server (LocalDB) со строкой из DbSettings.
    /// </summary>
    /// <param name="optionsBuilder">Построитель параметров контекста.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(DbSettings.ConnectionString);
    }

    /// <summary>
    /// Описывает модель (Fluent API): имя таблицы, первичный ключ, длины и обязательность полей, имя колонки GroupName.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("Students");
            entity.HasKey(s => s.ID);
            entity.Property(s => s.ID).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(s => s.Name).HasMaxLength(200).IsRequired();
            entity.Property(s => s.Speciality).HasMaxLength(200).IsRequired();
            // GROUP - зарезервированное слово SQL, поэтому колонка называется GroupName
            entity.Property(s => s.Group).HasColumnName("GroupName").HasMaxLength(50).IsRequired();
        });
        modelBuilder.Entity<StudentGrade>(entity =>
        {
            entity.ToTable("StudentGrades");
            entity.HasKey(g => g.ID);
            entity.Property(g => g.ID).HasColumnName("Id").ValueGeneratedOnAdd();
            entity.Property(g => g.StudentID).HasColumnName("StudentId");
            entity.Property(g => g.Subject).HasMaxLength(200).IsRequired();
            entity.HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
