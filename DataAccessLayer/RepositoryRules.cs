using Model;

namespace DataAccessLayer;

internal static class RepositoryRules
{
    public static void Validate(Student item, bool isNew)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (isNew ? item.ID != 0 : item.ID <= 0)
            throw new ArgumentException(isNew
                ? "Новый объект должен иметь ID = 0: идентификатор выдаёт база."
                : "Для обновления нужен ID существующего объекта.", nameof(item));
    }
}
