using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов через EF Core.</summary>
public sealed class EntityRepository : IRepository
{
    public int Add(IDomainObject item)
    {
        RepositoryRules.Validate(item, true);
        using var db = new Context();
        db.Students.Add((Student)item);
        db.SaveChanges();
        return item.ID;
    }

    public bool Delete<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return false;
        using var db = new Context();
        return db.Set<T>().Where(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id)
            .ExecuteDelete() == 1;
    }

    public List<T> GetAll<T>() where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        using var db = new Context();
        return db.Set<T>().AsNoTracking()
            .OrderBy(item => EF.Property<int>(item, nameof(IDomainObject.ID))).ToList();
    }

    public T? GetById<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return null;
        using var db = new Context();
        return db.Set<T>().AsNoTracking()
            .SingleOrDefault(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id);
    }

    public void Save(IDomainObject item)
    {
        RepositoryRules.Validate(item, false);
        using var db = new Context();
        var stored = db.Students.SingleOrDefault(s => s.ID == item.ID)
            ?? throw new KeyNotFoundException("Студент не найден.");
        db.Entry(stored).CurrentValues.SetValues((Student)item);
        db.SaveChanges();
    }
}
