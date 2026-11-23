namespace Model;

/// <summary>Общий контракт доменных объектов с идентификатором, выдаваемым БД.</summary>
public interface IDomainObject
{
    /// <summary>Получает или задаёт идентификатор доменного объекта, выданный базой данных.</summary>
    int ID { get; set; }
}
