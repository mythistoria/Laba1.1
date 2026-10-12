using Model;
using BL;
using TestData;

namespace View;
// сделал в привычной форме, мне так удобней
internal class Program
{
    static BusinessLogic logic = new BusinessLogic();

    static void Main(string[] args)
    {

        string[] items =
        {
            "Показать таблицу",
            "Добавить студента",
            "Удалить студента",
            "Показать гистограмму",
            "Выход"
        };

        int selected = 0;

        while (true)
        {
            DrawMenu(items, selected);
            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                selected = (selected - 1 + items.Length) % items.Length;
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                selected = (selected + 1) % items.Length;
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();

                switch (selected)
                {
                    case 0: ShowStudents(); break;
                    case 1: AddStudent(); break;
                    case 2: DeleteStudent(); break;
                    case 3: ShowHistogram(); break;
                    case 4: return;
                }

                Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                Console.ReadKey();
            }
        }
    }

    static void DrawMenu(string[] items, int selected)
    {
        Console.Clear();
        Console.WriteLine("Управление: стрелочки или W S, подтверждение: Enter\n");

        for (int i = 0; i < items.Length; i++)
        {
            if (i == selected)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($" ~ {items[i]} ");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {items[i]}");
            }
        }
    }//вывод меню выбора

    static void ShowStudents()
    {
        IReadOnlyList<Student> students = logic.GetStudents();

        if (students.Count == 0)
        {
            Console.WriteLine("Список студентов пуст");
            return;
        }

        Console.WriteLine($"{"ID",-4}{"Имя",-25}{"Специальность",-25}{"Группа",-10}");
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i,-4}{students[i].Name,-25}{students[i].Speciality,-25}{students[i].Group,-10}");
        }
    }//метод вывода студентов для консоли, использует GetStudents из BusinessLogic

    static void AddStudent()
    {
        Console.Write("ФИО: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Специальность: ");
        string speciality = Console.ReadLine() ?? "";
        Console.Write("Группа: ");
        string group = Console.ReadLine() ?? "";

        bool added = logic.AddStudent(name,speciality,group);

        if (added == true)
        {
            Console.WriteLine("Студент добавлен");
        }
        else
        {
            Console.WriteLine("Ошибка: заполните все поля");
        }

    }//метод добавления студента для консоли, использует AddStudent из BusinessLogic

    static void DeleteStudent()
    {
        ShowStudents();
        Console.Write("\nВведите ID студента для удаления: ");

        if (int.TryParse(Console.ReadLine(), out int id) && logic.DeleteStudent(id))
            Console.WriteLine("Студент удалён");
        else
            Console.WriteLine("Студента с таким ID нет");
    }//метод удаления студента для консоли, использует DeleteStudent из BusinessLogic

    static void ShowHistogram()
    {
        Dictionary<string, int> histogram = logic.GetHistogram();

        if (histogram.Count == 0)
        {
            Console.WriteLine("Нет данных для гистограммы");
            return;
        }

        Console.WriteLine("Распределение студентов по специальностям\n");
        foreach (KeyValuePair<string, int> pair in histogram)
        {
            Console.WriteLine($"{pair.Key,-25} {new string('#', pair.Value)} ({pair.Value})");
        }
    }//метод вывода гистограммы для консоли, использует GetHistogram из BusinessLogic
}