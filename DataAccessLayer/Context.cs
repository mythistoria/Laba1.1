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
            entity.Property(s => s.Group).HasColumnName("GroupName").HasMaxLength(50).IsRequired();
        });
    }
}
