namespace DataAccessLayer;

/// <summary>
/// Настройки подключения к базе данных.
/// </summary>
public static class DbSettings
{
    /// <summary>
    /// Имя экземпляра LocalDB (стандартный - MSSQLLocalDB).
    /// </summary>
    public const string Instance = "Laba2";

    /// <summary>
    /// Имя базы данных внутри экземпляра.
    /// </summary>
    public const string Database = "Laba2Students";

    /// <summary>
    /// Полный путь к файлу базы Laba2DB.mdf в папке проекта DataAccessLayer.
    /// </summary>
    public static readonly string DbFile = FindDbFile();

    // Рабочая строка подключения: база уже прикреплена к экземпляру, файл здесь не указываем.
    /// <summary>
    /// Рабочая строка подключения к базе (используют EF и Dapper).
    /// </summary>
    public static readonly string ConnectionString =
        $@"Server=(LocalDB)\{Instance};Database={Database};Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30";

    // Строка для служебного подключения к master: создание/прикрепление базы (см. DbInitializer).
    /// <summary>
    /// Строка подключения к системной базе master. Нужна, чтобы создать или прикрепить нашу базу.
    /// </summary>
    public static readonly string MasterConnectionString =
        $@"Server=(LocalDB)\{Instance};Database=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=30";

    /// <summary>
    /// Ищет папку DataAccessLayer, поднимаясь от папки с exe вверх по каталогам, и возвращает путь к файлу базы.
    /// </summary>
    /// <returns>Полный путь к Laba2DB.mdf.</returns>
    /// <summary>Ищет каталог DataAccessLayer и возвращает полный путь к MDF-файлу базы.</summary>
    /// <returns>Путь к Laba2DB.mdf.</returns>
    private static string FindDbFile()
    {
        DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            string projectDir = Path.Combine(dir.FullName, "DataAccessLayer");
            if (File.Exists(Path.Combine(projectDir, "DataAccessLayer.csproj")))
                return Path.Combine(projectDir, "Laba2DB.mdf");

            dir = dir.Parent;
        }

        // запасной вариант: рядом с exe
        return Path.Combine(AppContext.BaseDirectory, "Laba2DB.mdf");
    }
}
