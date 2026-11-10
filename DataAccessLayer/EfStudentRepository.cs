using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>
/// Репозиторий студентов на Entity Framework Core (сценарий 1).
/// </summary>
public class EfStudentRepository : IRepository<Student>
{
    /// <summary>
    /// Добавляет студента через DbSet.Add и SaveChanges.
    /// </summary>
    /// <param name="item">Новый студент.</param>
    /// <returns>Id, выданный базой.</returns>
    public int Create(Student item)
    {
        using var db = new AppDbContext();
        db.Students.Add(item);
        db.SaveChanges();
        return item.Id;
    }

    /// <summary>
    /// Читает всех студентов без отслеживания изменений (AsNoTracking), по возрастанию Id.
    /// </summary>
    /// <returns>Список студентов.</returns>
    public List<Student> ReadAll()
    {
        using var db = new AppDbContext();
        return db.Students.AsNoTracking().OrderBy(s => s.Id).ToList();
    }

    /// <summary>
    /// Ищет студента по Id.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Студент или null.</returns>
    public Student? ReadById(int id)
    {
        using var db = new AppDbContext();
        return db.Students.AsNoTracking().FirstOrDefault(s => s.Id == id);
    }

    /// <summary>
    /// Удаляет студента одним запросом DELETE (ExecuteDelete), не загружая объект в память.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public void Delete(int id)
    {
        using var db = new AppDbContext();
        db.Students.Where(s => s.Id == id).ExecuteDelete();
    }
}
