using Dapper;
using Microsoft.Data.SqlClient;
using Model;

namespace DataAccessLayer;

/// <summary>
/// Репозиторий студентов на Dapper (сценарий 2): SQL-запросы пишутся вручную.
/// </summary>
public class DapperStudentRepository : IRepository<Student>
{
    // колонка GroupName переименовывается в свойство Group
    private const string SelectSql =
        "SELECT Id, Name, Speciality, GroupName AS [Group] FROM Students";

    /// <summary>
    /// Вставляет студента запросом INSERT и получает Id через SCOPE_IDENTITY().
    /// </summary>
    /// <param name="item">Новый студент.</param>
    /// <returns>Id, выданный базой.</returns>
    public int Create(Student item)
    {
        using var connection = new SqlConnection(DbSettings.ConnectionString);

        const string sql =
            "INSERT INTO Students (Name, Speciality, GroupName) " +
            "VALUES (@Name, @Speciality, @Group); " +
            "SELECT CAST(SCOPE_IDENTITY() AS int);";

        return connection.QuerySingle<int>(sql, item);
    }

    /// <summary>
    /// Читает всех студентов запросом SELECT (колонка GroupName переименована в Group).
    /// </summary>
    /// <returns>Список студентов.</returns>
    public List<Student> ReadAll()
    {
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.Query<Student>(SelectSql + " ORDER BY Id").ToList();
    }

    /// <summary>
    /// Читает одного студента по Id (параметризованный запрос).
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Студент или null.</returns>
    public Student? ReadById(int id)
    {
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        return connection.QueryFirstOrDefault<Student>(SelectSql + " WHERE Id = @id", new { id });
    }

    /// <summary>
    /// Удаляет студента запросом DELETE с параметром.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public void Delete(int id)
    {
        using var connection = new SqlConnection(DbSettings.ConnectionString);
        connection.Execute("DELETE FROM Students WHERE Id = @id", new { id });
    }
}
