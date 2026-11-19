using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов через EF Core.</summary>
public sealed class EntityRepository : IRepository<Student>
{
    /// <summary>Добавляет студента через EF Core и возвращает выданный ID.</summary>
    /// <param name="item">Новый студент.</param>
    /// <returns>Идентификатор добавленного студента.</returns>
    public int Add(Student item)
    {
        RepositoryRules.Validate(item, true);
        using var db = new Context();
        db.Students.Add(item);
        db.SaveChanges();
        return item.ID;
    }

    /// <summary>Удаляет студента с указанным идентификатором через EF Core.</summary>
    /// <param name="id">ID студента.</param>
    /// <returns>true, если удалена одна запись; иначе false.</returns>
    public bool Delete(int id)
    {
        if (id <= 0) return false;
        using var db = new Context();
        return db.Students.Where(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id)
            .ExecuteDelete() == 1;
    }

    /// <summary>Возвращает всех студентов без отслеживания изменений, отсортированных по ID.</summary>
    /// <returns>Список студентов.</returns>
    public List<Student> GetAll()
    {
        using var db = new Context();
        return db.Students.AsNoTracking()
            .OrderBy(item => EF.Property<int>(item, nameof(IDomainObject.ID))).ToList();
    }

    /// <summary>Возвращает студента с указанным идентификатором через EF Core.</summary>
    /// <param name="id">ID студента.</param>
    /// <returns>Студент или null, если запись не найдена.</returns>
    public Student? GetById(int id)
    {
        if (id <= 0) return null;
        using var db = new Context();
        return db.Students.AsNoTracking()
            .SingleOrDefault(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id);
    }

    /// <summary>Обновляет данные существующего студента через EF Core.</summary>
    /// <param name="item">Студент с существующим ID и обновленными данными.</param>
    public void Save(Student item)
    {
        RepositoryRules.Validate(item, false);
        using var db = new Context();
        var stored = db.Students.SingleOrDefault(s => s.ID == item.ID)
            ?? throw new KeyNotFoundException("Студент не найден.");
        db.Entry(stored).CurrentValues.SetValues(item);
        db.SaveChanges();
    }
}
