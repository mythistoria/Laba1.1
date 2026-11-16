using Dapper;
using Microsoft.Data.SqlClient;
using Model;

namespace DataAccessLayer;

/// <summary>Тот же CRUD-контракт через явные параметризованные SQL-запросы.</summary>
public sealed class DapperRepository : IRepository
{
    public int Add(IDomainObject item)
    {
        RepositoryRules.Validate(item, true);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        connection.Open();
        if (item is Student student)
        {
            using var transaction = connection.BeginTransaction();
            int id = connection.QuerySingle<int>(
                "INSERT INTO Students (Name, Speciality, GroupName) VALUES (@Name, @Speciality, @Group); " +
                "SELECT CAST(SCOPE_IDENTITY() AS int);", student, transaction);
            var added = student.Grades.Select(grade => (Grade: grade,
                ID: InsertGrade(connection, transaction, grade, id))).ToList();
            transaction.Commit();
            student.ID = id;
            foreach (var pair in added)
            {
                pair.Grade.ID = pair.ID;
                pair.Grade.StudentID = id;
                pair.Grade.Student = student;
            }
        }
        else if (item is StudentGrade grade)
        {
            grade.ID = InsertGrade(connection, null, grade, grade.StudentID);
        }
        return item.ID;
    }

    public bool Delete<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return false;
        string table = typeof(T) == typeof(Student) ? "Students" : "StudentGrades";
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        // Имя таблицы выбирается только из двух констант, ID передаётся параметром.
        return connection.Execute($"DELETE FROM {table} WHERE Id = @ID", new { ID = id }) == 1;
    }

    public List<T> GetAll<T>() where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        var students = ReadStudents(connection, null);
        return typeof(T) == typeof(Student)
            ? students.Cast<T>().ToList()
            : students.SelectMany(s => s.Grades).OrderBy(g => g.ID).Cast<T>().ToList();
    }

    public T? GetById<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return null;
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        if (typeof(T) == typeof(Student)) return ReadStudents(connection, id).Cast<T>().SingleOrDefault();
        int? studentId = connection.QuerySingleOrDefault<int?>(
            "SELECT StudentId FROM StudentGrades WHERE Id = @ID", new { ID = id });
        return studentId.HasValue
            ? ReadStudents(connection, studentId.Value).SelectMany(s => s.Grades).Where(g => g.ID == id).Cast<T>().SingleOrDefault()
            : null;
    }

    public void Save(IDomainObject item)
    {
        RepositoryRules.Validate(item, false);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        connection.Open();
        if (item is Student student)
        {
            using var transaction = connection.BeginTransaction();
            if (connection.Execute("UPDATE Students SET Name = @Name, Speciality = @Speciality, GroupName = @Group WHERE Id = @ID",
                    student, transaction) != 1)
                throw new KeyNotFoundException("Студент не найден.");

            var existingIds = connection.Query<int>("SELECT Id FROM StudentGrades WHERE StudentId = @ID",
                new { student.ID }, transaction).ToHashSet();
            var incomingIds = student.Grades.Where(g => g.ID > 0).Select(g => g.ID).ToHashSet();
            if (incomingIds.Any(id => !existingIds.Contains(id)))
                throw new ArgumentException("Сохранение не может присвоить студенту чужую или отсутствующую оценку.");
            foreach (int removedId in existingIds.Except(incomingIds))
                connection.Execute("DELETE FROM StudentGrades WHERE Id = @ID AND StudentId = @StudentID",
                    new { ID = removedId, StudentID = student.ID }, transaction);

            var added = new List<(StudentGrade Grade, int ID)>();
            foreach (var grade in student.Grades)
            {
                if (grade.ID == 0) added.Add((grade, InsertGrade(connection, transaction, grade, student.ID)));
                else UpdateGrade(connection, transaction, grade, student.ID);
            }
            transaction.Commit();
            foreach (var pair in added) pair.Grade.ID = pair.ID;
            foreach (var grade in student.Grades)
            {
                grade.StudentID = student.ID;
                grade.Student = student;
            }
        }
        else if (item is StudentGrade grade)
        {
            UpdateGrade(connection, null, grade, grade.StudentID);
        }
    }

    private static int InsertGrade(SqlConnection connection, SqlTransaction? transaction, StudentGrade grade, int studentId) =>
        connection.QuerySingle<int>(
            "INSERT INTO StudentGrades (StudentId, Subject, [Value]) VALUES (@StudentID, @Subject, @Value); " +
            "SELECT CAST(SCOPE_IDENTITY() AS int);",
            new { StudentID = studentId, grade.Subject, grade.Value }, transaction);

    private static void UpdateGrade(SqlConnection connection, SqlTransaction? transaction, StudentGrade grade, int studentId)
    {
        if (connection.Execute("UPDATE StudentGrades SET Subject = @Subject, [Value] = @Value WHERE Id = @ID AND StudentId = @StudentID",
                new { grade.ID, StudentID = studentId, grade.Subject, grade.Value }, transaction) != 1)
            throw new KeyNotFoundException("Оценка не найдена у указанного студента.");
    }

    private static List<Student> ReadStudents(SqlConnection connection, int? studentId)
    {
        string filter = studentId.HasValue ? " WHERE s.Id = @ID" : "";
        string sql = "SELECT s.Id AS ID, s.Name, s.Speciality, s.GroupName AS [Group] FROM Students s" + filter + " ORDER BY s.Id; " +
            "SELECT g.Id AS ID, g.StudentId AS StudentID, g.Subject, g.[Value] " +
            "FROM StudentGrades g INNER JOIN Students s ON s.Id = g.StudentId" + filter + " ORDER BY g.Id;";
        using var result = connection.QueryMultiple(sql, new { ID = studentId });
        var students = result.Read<Student>().ToList();
        var owners = students.ToDictionary(s => s.ID);
        foreach (var grade in result.Read<StudentGrade>())
        {
            if (owners.TryGetValue(grade.StudentID, out var student))
            {
                grade.Student = student;
                student.Grades.Add(grade);
            }
        }
        return students;
    }
}
