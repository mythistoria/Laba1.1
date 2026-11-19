using DataAccessLayer;
using Model;

namespace BL;

/// <summary>
/// Режим хранения данных: через Entity Framework Core или через Dapper.
/// </summary>
public enum StorageMode
{
    /// <summary>Использовать Entity Framework Core.</summary>
    EfCore,

    /// <summary>Использовать Dapper.</summary>
    Dapper
}

/// <summary>
/// Бизнес-логика приложения: проверка данных, добавление, удаление, список и статистика. Не зависит от интерфейса.
/// </summary>
public class BusinessLogic
{
    private readonly IRepository<Student> repository;

    /// <summary>
    /// Инициализирует базу данных и выбирает реализацию репозитория.
    /// </summary>
    /// <param name="mode">EF Core (по умолчанию) или Dapper.</param>
    public BusinessLogic(StorageMode mode = StorageMode.EfCore)
    {
        DbInitializer.Init();

        repository = mode == StorageMode.Dapper
            ? new DapperRepository()
            : new EntityRepository();
    }

    /// <summary>
    /// Проверяет, что поля не пустые, и добавляет студента.
    /// </summary>
    /// <param name="newName">ФИО.</param>
    /// <param name="newSpeciality">Специальность.</param>
    /// <param name="newGroup">Группа.</param>
    /// <returns>true - студент добавлен; false - данные некорректны.</returns>
    public bool AddStudent(string newName, string newSpeciality, string newGroup)
    {
        if (string.IsNullOrWhiteSpace(newName) ||
            string.IsNullOrWhiteSpace(newSpeciality) ||
            string.IsNullOrWhiteSpace(newGroup))
        {
            return false;
        }

        repository.Add(new Student
        {
            Name = newName.Trim(),
            Speciality = newSpeciality.Trim(),
            Group = newGroup.Trim()
        });

        return true;
    }

    /// <summary>
    /// Удаляет студента по номеру в списке: находит его Id и вызывает репозиторий.
    /// </summary>
    /// <param name="index">Номер строки в списке, начиная с 0.</param>
    /// <returns>true - удалено; false - номер вне диапазона.</returns>
    public bool DeleteStudent(int index)
    {
        List<Student> students = repository.GetAll();

        if (index < 0 || index >= students.Count)
        {
            return false;
        }

        return repository.Delete(students[index].ID);
    }

    /// <summary>
    /// Возвращает список студентов как массив массивов строк (ID, ФИО, специальность, группа).
    /// </summary>
    /// <returns>Массив строк [ID, ФИО, специальность, группа].</returns>
    public string[][] GetStudents()
    {
        List<Student> students = repository.GetAll();
        string[][] result = new string[students.Count][];

        for (int i = 0; i < students.Count; i++)
        {
            result[i] = new string[]
            {
                students[i].ID.ToString(),
                students[i].Name,
                students[i].Speciality,
                students[i].Group
            };
        }

        return result;
    }

    /// <summary>
    /// Удаляет студента по реальному идентификатору базы данных.
    /// </summary>
    /// <param name="id">ID студента.</param>
    /// <returns>true - удалено; false - ID не существует.</returns>
    public bool DeleteStudentById(int id)
    {
        return repository.Delete(id);
    }

    /// <summary>
    /// Считает количество студентов по каждой специальности.
    /// </summary>
    /// <returns>Словарь: специальность - количество.</returns>
    public Dictionary<string, int> GetHistogram()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (Student student in repository.GetAll())
        {
            if (!result.ContainsKey(student.Speciality))
                result.Add(student.Speciality, 1);
            else
                result[student.Speciality]++;
        }

        return result;
    }
}
