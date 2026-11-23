using Model;

namespace DataAccessLayer;

/// <summary>Проверяет допустимость данных перед обращением репозитория к базе.</summary>
internal static class RepositoryRules
{
    /// <summary>Проверяет идентификатор, обязательные поля и ограничения длины.</summary>
    /// <param name="item">Проверяемый студент.</param>
    /// <param name="isNew">true для добавления нового объекта; false для обновления существующего.</param>
    public static void Validate(Student item, bool isNew)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (isNew ? item.ID != 0 : item.ID <= 0)
            throw new ArgumentException(isNew
                ? "Новый объект должен иметь ID = 0: идентификатор выдаёт база."
                : "Для обновления нужен ID существующего объекта.", nameof(item));

        item.Name = item.Name.Trim();
        item.Speciality = item.Speciality.Trim();
        item.Group = item.Group.Trim();

        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ArgumentException("ФИО не может быть пустым.", nameof(item));
        if (string.IsNullOrWhiteSpace(item.Speciality))
            throw new ArgumentException("Специальность не может быть пустой.", nameof(item));
        if (string.IsNullOrWhiteSpace(item.Group))
            throw new ArgumentException("Группа не может быть пустой.", nameof(item));

        if (item.Name.Length > 200)
            throw new ArgumentException("ФИО не может превышать 200 символов.", nameof(item));
        if (item.Speciality.Length > 200)
            throw new ArgumentException("Специальность не может превышать 200 символов.", nameof(item));
        if (item.Group.Length > 50)
            throw new ArgumentException("Название группы не может превышать 50 символов.", nameof(item));
    }
}
