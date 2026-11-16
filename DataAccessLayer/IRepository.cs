using Model;

namespace DataAccessLayer;

/// <summary>Репозиторий только для ссылочных доменных типов с обязательным ID.</summary>
public interface IRepository<T> where T : class, IDomainObject
{
    /// <summary>Добавляет новый объект и присваивает ему ID, выданный БД.</summary>
    int Add(T item);
    bool Delete(int id);
    List<T> GetAll();
    T? GetById(int id);
    /// <summary>Обновляет существующий объект.</summary>
    void Save(T item);
}
