using Model;
namespace BL;

public class BusinessLogic
{
    private List<Student> students = new List<Student>();

    public void AddTestStudents() //добавил в логику тестовые данные, удалил тестовые данные из View
    {
        AddStudent("Иван Иванов", "Программирование", "ПИ-21");
        AddStudent("Алексей Петров", "Программирование", "ПИ-21");
        AddStudent("Максим Сидоров", "Информационные системы", "ИС-22");
        AddStudent("Анна Смирнова", "Экономика", "ЭК-21");
        AddStudent("Дмитрий Кузнецов", "Информационные системы", "ИС-22");
        AddStudent("Мария Попова", "Экономика", "ЭК-21");
        AddStudent("Никита Волков", "Программирование", "ПИ-23");
        AddStudent("Екатерина Соколова", "Информационные системы", "ИС-22");
        AddStudent("Артём Морозов", "Программирование", "ПИ-23");
        AddStudent("Ольга Новикова", "Экономика", "ЭК-21");
    }
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

    public bool DeleteStudent(int index)
    {
        try
        {
            students.RemoveAt(index);
            return true;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    public string[][] GetStudents() // теперь возвращает массив массивов, чтобы не использовать обьекты student
    {
        var result = new string[students.Count][];
        for (int i = 0; i < students.Count; i++)
        {
            result[i] = new string[]
            {
                students[i].Name,
                students[i].Speciality,
                students[i].Group
            };
        }
        return result;
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