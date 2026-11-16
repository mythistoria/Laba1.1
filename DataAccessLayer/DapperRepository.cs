using Dapper;
using Microsoft.Data.SqlClient;
using Model;

namespace DataAccessLayer;

/// <summary>CRUD студентов через параметризованные SQL-запросы.</summary>
public sealed class DapperRepository : IRepository
{
    private const string SelectSql = "SELECT Id AS ID, Name, Speciality, GroupName AS [Group] FROM Students";

    public int Add<T>(T item) where T : class, IDomainObject
    {
        var student = RepositoryRules.Validate(item, true);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        item.ID = connection.QuerySingle<int>(
            "INSERT INTO Students (Name, Speciality, GroupName) VALUES (@Name, @Speciality, @Group); " +
            "SELECT CAST(SCOPE_IDENTITY() AS int);", student);
        return item.ID;
    }

    public bool Delete<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return false;
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.Execute("DELETE FROM Students WHERE Id = @ID", new { ID = id }) == 1;
    }

    public List<T> GetAll<T>() where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.Query<T>(SelectSql + " ORDER BY Id").ToList();
    }

    public T? GetById<T>(int id) where T : class, IDomainObject
    {
        RepositoryRules.EnsureSupported<T>();
        if (id <= 0) return null;
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.QuerySingleOrDefault<T>(SelectSql + " WHERE Id = @ID", new { ID = id });
    }

    public void Save<T>(T item) where T : class, IDomainObject
    {
        var student = RepositoryRules.Validate(item, false);
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        if (connection.Execute("UPDATE Students SET Name = @Name, Speciality = @Speciality, GroupName = @Group WHERE Id = @ID",
                student) != 1)
            throw new KeyNotFoundException("Студент не найден.");
    }
}
