using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов через EF Core.</summary>
public sealed class EntityRepository : IRepository<Student>
{
    public int Add(Student item)
    {
        RepositoryRules.Validate(item, true);
        using var db = new Context();
        db.Students.Add(item);
        db.SaveChanges();
        return item.ID;
    }

    public bool Delete(int id)
    {
        if (id <= 0) return false;
        using var db = new Context();
        return db.Students.Where(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id)
            .ExecuteDelete() == 1;
    }

    public List<Student> GetAll()
    {
        using var db = new Context();
        return db.Students.AsNoTracking()
            .OrderBy(item => EF.Property<int>(item, nameof(IDomainObject.ID))).ToList();
    }

    public Student? GetById(int id)
    {
        if (id <= 0) return null;
        using var db = new Context();
        return db.Students.AsNoTracking()
            .SingleOrDefault(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id);
    }

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
