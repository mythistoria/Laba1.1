using Dapper;
using Microsoft.Data.SqlClient;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов через параметризованные SQL-запросы.</summary>
public sealed class DapperRepository : IRepository<Student>
{
    private const string SelectSql = "SELECT Id AS ID, Name, Speciality, GroupName AS [Group] FROM Students";

    public int Add(Student item)
    {
        RepositoryRules.Validate(item, true);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        item.ID = connection.QuerySingle<int>(
            "INSERT INTO Students (Name, Speciality, GroupName) VALUES (@Name, @Speciality, @Group); " +
            "SELECT CAST(SCOPE_IDENTITY() AS int);", item);
        return item.ID;
    }

    public bool Delete(int id)
    {
        if (id <= 0) return false;
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.Execute("DELETE FROM Students WHERE Id = @ID", new { ID = id }) == 1;
    }

    public List<Student> GetAll()
    {
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.Query<Student>(SelectSql + " ORDER BY Id").ToList();
    }

    public Student? GetById(int id)
    {
        if (id <= 0) return null;
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.QuerySingleOrDefault<Student>(SelectSql + " WHERE Id = @ID", new { ID = id });
    }

    public void Save(Student item)
    {
        RepositoryRules.Validate(item, false);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        if (connection.Execute("UPDATE Students SET Name = @Name, Speciality = @Speciality, GroupName = @Group WHERE Id = @ID",
                item) != 1)
            throw new KeyNotFoundException("Студент не найден.");
    }
}
