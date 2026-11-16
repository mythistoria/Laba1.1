using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов и оценок через EF Core.</summary>
public sealed class EntityRepository : IRepository
{
    public int Add(IDomainObject item)
    {
        RepositoryRules.Validate(item, true);
        using var db = new Context();
        if (item is Student student)
        {
            foreach (var grade in student.Grades) grade.Student = student;
            db.Students.Add(student);
        }
        else if (item is StudentGrade grade)
        {
            // Не добавляем повторно объект владельца, переданный в навигационном свойстве.
            db.Entry(grade).State = EntityState.Added;
        }
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
        using var db = new Context();
        return ReadQuery<T>(db).OrderBy(item => EF.Property<int>(item, nameof(IDomainObject.ID))).ToList();
    }

    public T? GetById<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return null;
        using var db = new Context();
        return ReadQuery<T>(db).SingleOrDefault(item => EF.Property<int>(item, nameof(IDomainObject.ID)) == id);
    }

    public void Save(IDomainObject item)
    {
        RepositoryRules.Validate(item, false);
        using var db = new Context();
        if (item is Student student)
        {
            var stored = db.Students.Include(s => s.Grades).SingleOrDefault(s => s.ID == student.ID)
                ?? throw new KeyNotFoundException("Студент не найден.");
            var existing = stored.Grades.ToDictionary(g => g.ID);
            var incomingIds = student.Grades.Where(g => g.ID > 0).Select(g => g.ID).ToHashSet();
            if (incomingIds.Any(id => !existing.ContainsKey(id)))
                throw new ArgumentException("Сохранение не может присвоить студенту чужую или отсутствующую оценку.");

            db.Entry(stored).CurrentValues.SetValues(student);
            foreach (var removed in stored.Grades.Where(g => !incomingIds.Contains(g.ID)).ToList())
                db.StudentGrades.Remove(removed);

            var added = new List<(StudentGrade Input, StudentGrade Stored)>();
            foreach (var grade in student.Grades)
            {
                if (grade.ID > 0)
                {
                    existing[grade.ID].Subject = grade.Subject;
                    existing[grade.ID].Value = grade.Value;
                }
                else
                {
                    var newGrade = new StudentGrade { Subject = grade.Subject, Value = grade.Value, Student = stored };
                    db.StudentGrades.Add(newGrade);
                    added.Add((grade, newGrade));
                }
            }
            // Все изменения студента и состава оценок применяются одним SaveChanges.
            db.SaveChanges();
            foreach (var pair in added) pair.Input.ID = pair.Stored.ID;
            foreach (var grade in student.Grades)
            {
                grade.StudentID = student.ID;
                grade.Student = student;
            }
        }
        else if (item is StudentGrade grade)
        {
            var stored = db.StudentGrades.SingleOrDefault(g => g.ID == grade.ID)
                ?? throw new KeyNotFoundException("Оценка не найдена.");
            if (stored.StudentID != grade.StudentID)
                throw new ArgumentException("Нельзя перенести оценку другому студенту.");
            stored.Subject = grade.Subject;
            stored.Value = grade.Value;
            db.SaveChanges();
        }
    }

    private static IQueryable<T> ReadQuery<T>(Context db) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (typeof(T) == typeof(Student))
            return db.Students.Include(s => s.Grades).AsNoTrackingWithIdentityResolution().Cast<T>();
        return db.StudentGrades.Include(g => g.Student).ThenInclude(s => s.Grades)
            .AsNoTrackingWithIdentityResolution().Cast<T>();
    }
}
