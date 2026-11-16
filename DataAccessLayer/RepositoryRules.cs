using Model;

namespace DataAccessLayer;

internal static class RepositoryRules
{
    public static void EnsureSupported<T>() where T : class, IDomainObject
    {
        if (typeof(T) != typeof(Student))
            throw new NotSupportedException($"Тип {typeof(T).Name} не входит в модель хранилища.");
    }

    public static void Validate(IDomainObject item, bool isNew)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (item is not Student)
            throw new NotSupportedException($"Тип {item.GetType().Name} не входит в модель хранилища.");
        if (isNew ? item.ID != 0 : item.ID <= 0)
            throw new ArgumentException(isNew
                ? "Новый объект должен иметь ID = 0: идентификатор выдаёт база."
                : "Для обновления нужен ID существующего объекта.", nameof(item));
    }
}
