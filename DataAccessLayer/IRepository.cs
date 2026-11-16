using Model;

namespace DataAccessLayer;

/// <summary>Общий CRUD-контракт. Методы принимают только доменные объекты с ID.</summary>
public interface IRepository
{
    /// <summary>Добавляет новый объект и присваивает ему ID, выданный БД.</summary>
    int Add(IDomainObject item);
    bool Delete<T>(int id) where T : class, IDomainObject;
    List<T> GetAll<T>() where T : class, IDomainObject;
    T? GetById<T>(int id) where T : class, IDomainObject;
    /// <summary>Обновляет существующий объект.</summary>
    void Save(IDomainObject item);
}
