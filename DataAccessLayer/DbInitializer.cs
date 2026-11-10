using Microsoft.Data.SqlClient;
using Model;

namespace DataAccessLayer;

/// <summary>
/// Готовит базу к работе при запуске: создаёт или прикрепляет базу, создаёт таблицу и добавляет тестовые данные.
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Подготавливает базу: EnsureDatabase, затем создание таблицы (EnsureCreated) и добавление 10 тестовых студентов, если таблица пуста. Можно вызывать многократно.
    /// </summary>
    public static void Init()
    {
        EnsureDatabase();

        using var db = new AppDbContext();
        db.Database.EnsureCreated();

        if (db.Students.Any())
            return;

        db.Students.AddRange(
            new Student { Name = "Иван Иванов",         Speciality = "Программирование",        Group = "ПИ-21" },
            new Student { Name = "Алексей Петров",      Speciality = "Программирование",        Group = "ПИ-21" },
            new Student { Name = "Максим Сидоров",      Speciality = "Информационные системы", Group = "ИС-22" },
            new Student { Name = "Анна Смирнова",       Speciality = "Экономика",               Group = "ЭК-21" },
            new Student { Name = "Дмитрий Кузнецов",    Speciality = "Информационные системы", Group = "ИС-22" },
            new Student { Name = "Мария Попова",        Speciality = "Экономика",               Group = "ЭК-21" },
            new Student { Name = "Никита Волков",       Speciality = "Программирование",        Group = "ПИ-23" },
            new Student { Name = "Екатерина Соколова",  Speciality = "Информационные системы", Group = "ИС-22" },
            new Student { Name = "Артём Морозов",       Speciality = "Программирование",        Group = "ПИ-23" },
            new Student { Name = "Ольга Новикова",      Speciality = "Экономика",               Group = "ЭК-21" });

        db.SaveChanges();
    }

    /// <summary>
    /// Проверяет через master, прикреплена ли база к экземпляру и к нужному файлу. При необходимости создаёт базу, прикрепляет существующий файл или откреплает старую запись и прикрепляет файл заново.
    /// </summary>
    private static void EnsureDatabase()
    {
        string name = DbSettings.Database;
        string file = DbSettings.DbFile;

        using var connection = new SqlConnection(DbSettings.MasterConnectionString);
        connection.Open();

        string? attachedFile;
        using (var cmd = connection.CreateCommand())
        {
            cmd.CommandText =
                "SELECT physical_name FROM sys.master_files WHERE database_id = DB_ID(@name) AND type = 0";
            cmd.Parameters.AddWithValue("@name", name);
            attachedFile = cmd.ExecuteScalar() as string;
        }

        if (attachedFile != null)
        {
            if (string.Equals(attachedFile, file, StringComparison.OrdinalIgnoreCase))
                return; // всё уже на месте

            // база с таким именем прикреплена к другому файлу (проект перенесли) - открепляем
            SqlConnection.ClearAllPools();
            using var detach = connection.CreateCommand();
            detach.CommandText = "EXEC sp_detach_db @dbname = @name";
            detach.Parameters.AddWithValue("@name", name);
            detach.ExecuteNonQuery();
        }

        string mdf = file.Replace("'", "''");
        string ldf = (Path.Combine(Path.GetDirectoryName(file)!, Path.GetFileNameWithoutExtension(file) + "_log.ldf"))
            .Replace("'", "''");

        string sql = File.Exists(file)
            ? $"CREATE DATABASE [{name}] ON (FILENAME = N'{mdf}') FOR ATTACH"
            : $"CREATE DATABASE [{name}] ON PRIMARY (NAME = N'{name}', FILENAME = N'{mdf}') " +
              $"LOG ON (NAME = N'{name}_log', FILENAME = N'{ldf}')";

        using var create = connection.CreateCommand();
        create.CommandText = sql;
        create.ExecuteNonQuery();
    }
}
