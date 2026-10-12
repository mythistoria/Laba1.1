using Model;
namespace BL;

public class BusinessLogic
{
    private List<Student> students = new List<Student>();

    public bool AddStudent(string newName, string newSpeciality, string newGroup)//заменил метод, чтобы в проверка вводимых данных осуществлялась в логике
    {
        if (string.IsNullOrWhiteSpace(newName) ||
            string.IsNullOrWhiteSpace(newSpeciality) ||
            string.IsNullOrWhiteSpace(newGroup))
        {
            return false;
        }
        Student newStudent = new Student();
        newStudent.Name = newName;
        newStudent.Speciality = newSpeciality;
        newStudent.Group = newGroup;
        students.Add(newStudent);
        return true;
    }
    public bool DeleteStudent(int index) //убрал Console.WriteLine для использования в Form, возвращает bool
    {
        if (index < 0 || index >= students.Count)
            return false;

        students.RemoveAt(index);
        return true;
    }
    //удалил метод ShowStudents, так как для Form и консоли нужны разные методы
    //в каждом проекте свои методы для вывода студентов
    public IReadOnlyList<Student> GetStudents()
    {
        return students.AsReadOnly();
    }

    public Dictionary<string, int> GetHistogram() //возвращает словарь вместо Console.WriteLine для использования в Form
    {
        Dictionary<string, int> result = new Dictionary<string, int>();

        foreach (Student student in students)
        {
            if (!result.ContainsKey(student.Speciality))
                result.Add(student.Speciality, 1);
            else
                result[student.Speciality]++;
        }

        return result;
    }
}