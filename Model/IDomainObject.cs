namespace Model;

/// <summary>Общий контракт доменных объектов с идентификатором, выдаваемым БД.</summary>
public interface IDomainObject
{
    int ID { get; set; }
}
