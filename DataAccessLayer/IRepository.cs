using Model;

namespace DataAccessLayer;

/// <summary>Репозиторий только для ссылочных доменных типов с обязательным ID.</summary>
public interface IRepository<T> where T : class, IDomainObject
{
    /// <summary>Добавляет новый объект и присваивает ему ID, выданный БД.</summary>
    /// <param name="item">Новый объект.</param>
    /// <returns>Идентификатор, выданный базой данных.</returns>
    int Add(T item);

    /// <summary>Удаляет объект по идентификатору.</summary>
    /// <param name="id">ID объекта.</param>
    /// <returns>true, если объект удалён; иначе false.</returns>
    bool Delete(int id);

    /// <summary>Возвращает все объекты в порядке возрастания ID.</summary>
    /// <returns>Список объектов.</returns>
    List<T> GetAll();

    /// <summary>Ищет объект по идентификатору.</summary>
    /// <param name="id">ID объекта.</param>
    /// <returns>Найденный объект или null.</returns>
    T? GetById(int id);
    /// <summary>Обновляет существующий объект.</summary>
    /// <param name="item">Объект с обновленными данными и существующим ID.</param>
    void Save(T item);
}
