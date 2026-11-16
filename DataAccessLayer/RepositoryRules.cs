using Model;

namespace DataAccessLayer;

internal static class RepositoryRules
{
    public static void EnsureSupported<T>() where T : class, IDomainObject
    {
        if (typeof(T) != typeof(Student) && typeof(T) != typeof(StudentGrade))
            throw new NotSupportedException($"Тип {typeof(T).Name} не входит в модель хранилища.");
    }

    public static void Validate(IDomainObject item, bool isNew)
    {
        ArgumentNullException.ThrowIfNull(item);
        if (isNew ? item.ID != 0 : item.ID <= 0)
            throw new ArgumentException(isNew
                ? "Новый объект должен иметь ID = 0: идентификатор выдаёт база."
                : "Для обновления нужен ID существующего объекта.", nameof(item));

        switch (item)
        {
            case Student student:
                ArgumentNullException.ThrowIfNull(student.Grades);
                var ids = new HashSet<int>();
                var objects = new HashSet<StudentGrade>(ReferenceEqualityComparer.Instance);
                foreach (var grade in student.Grades)
                {
                    ArgumentNullException.ThrowIfNull(grade);
                    if (!objects.Add(grade) || grade.ID < 0 ||
                        (grade.ID > 0 && (isNew || !ids.Add(grade.ID))))
                        throw new ArgumentException("Коллекция содержит повторную или некорректную оценку.");
                    if ((grade.StudentID != 0 && grade.StudentID != student.ID) ||
                        (grade.Student != null && !ReferenceEquals(grade.Student, student) &&
                         (student.ID == 0 || grade.Student.ID != student.ID)))
                        throw new ArgumentException("Оценка уже принадлежит другому студенту.");
                }
                break;
            case StudentGrade grade:
                if (grade.StudentID <= 0 || (grade.Student != null && grade.Student.ID != grade.StudentID))
                    throw new ArgumentException("Оценка должна принадлежать существующему студенту.");
                break;
            default:
                throw new NotSupportedException($"Тип {item.GetType().Name} не входит в модель хранилища.");
        }
    }
}
