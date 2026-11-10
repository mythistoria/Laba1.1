namespace DataAccessLayer;

/// <summary>Базовые CRUD-операции. Update пока не нужен.</summary>
public interface IRepository<T>
{
    /// <summary>
    /// Добавляет запись в хранилище.
    /// </summary>
    /// <param name="item">Добавляемый объект.</param>
    /// <returns>Идентификатор (Id), выданный базой данных.</returns>
    int Create(T item);
    /// <summary>
    /// Читает все записи, отсортированные по Id.
    /// </summary>
    /// <returns>Список всех объектов.</returns>
    List<T> ReadAll();
    /// <summary>
    /// Ищет запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор записи.</param>
    /// <returns>Найденный объект или null, если записи нет.</returns>
    T? ReadById(int id);
    /// <summary>
    /// Удаляет запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемой записи.</param>
    void Delete(int id);
}
