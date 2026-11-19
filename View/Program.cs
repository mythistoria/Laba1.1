using BL;

namespace View;
/// <summary>
/// Консольный интерфейс: меню со стрелками, работает через BusinessLogic.
/// </summary>
internal class Program
{
    static BusinessLogic logic = null!;
    
    /// <summary>
    /// Точка входа консольного приложения: выбирает режим хранения, подключается к базе и запускает цикл меню.
    /// </summary>
    /// <param name="args">Аргументы командной строки. Аргумент dapper выбирает Dapper.</param>
    static void Main(string[] args)
    {
        StorageMode mode = args.Any(arg => string.Equals(arg, "dapper", StringComparison.OrdinalIgnoreCase))
            ? StorageMode.Dapper
            : ChooseMode();

        try
        {
            logic = new BusinessLogic(mode);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Не удалось подключиться к базе данных:\n" + (ex.InnerException ?? ex).Message);
            return;
        }

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

                try
                {
                    switch (selected)
                    {
                        case 0: ShowStudents(); break;
                        case 1: AddStudent(); break;
                        case 2: DeleteStudent(); break;
                        case 3: ShowHistogram(); break;
                        case 4: return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка выполнения операции:\n" + (ex.InnerException ?? ex).Message);
                }

                Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в меню...");
                Console.ReadKey();
            }
        }
    }
    /// <summary>
    /// Показывает меню выбора реализации доступа к данным.
    /// </summary>
    /// <returns>Выбранный режим EF Core или Dapper.</returns>
    static StorageMode ChooseMode()
    {
        string[] items = { "Entity Framework Core", "Dapper" };
        int selected = 0;

        while (true)
        {
            DrawMenu(items, selected);
            Console.WriteLine("\n(выберите реализацию доступа к данным)");
            ConsoleKey key = Console.ReadKey().Key;

            if (key == ConsoleKey.UpArrow || key == ConsoleKey.W)
                selected = (selected - 1 + items.Length) % items.Length;
            else if (key == ConsoleKey.DownArrow || key == ConsoleKey.S)
                selected = (selected + 1) % items.Length;
            else if (key == ConsoleKey.Enter)
                return selected == 1 ? StorageMode.Dapper : StorageMode.EfCore;
        }
    }
    /// <summary>
    /// Рисует меню и подсвечивает выбранный пункт.
    /// </summary>
    /// <param name="items">Названия пунктов.</param>
    /// <param name="selected">Номер выбранного пункта.</param>
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

    /// <summary>
    /// Выводит таблицу студентов: номер, ФИО, специальность, группа.
    /// </summary>
    static void ShowStudents()
    {
        string[][] studentsData = logic.GetStudents();

        if (studentsData.Length == 0)
        {
            Console.WriteLine("Список студентов пуст");
            return;
        }

        Console.WriteLine($"{"ID",-4}{"Имя",-25}{"Специальность",-25}{"Группа",-10}");
        for (int i = 0; i < studentsData.Length; i++)
        {
            Console.WriteLine($"{studentsData[i][0],-4}{studentsData[i][1],-25}{studentsData[i][2],-25}{studentsData[i][3],-10}");
        }
    }//метод вывода студентов для консоли, использует GetStudents из BusinessLogic

    /// <summary>
    /// Запрашивает данные у пользователя и добавляет студента.
    /// </summary>
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

    /// <summary>
    /// Показывает список и удаляет студента по введённому номеру.
    /// </summary>
    static void DeleteStudent()
    {
        ShowStudents();
        Console.Write("\nВведите ID студента для удаления: ");

        if (int.TryParse(Console.ReadLine(), out int id) && logic.DeleteStudentById(id))
            Console.WriteLine("Студент удалён");
        else
            Console.WriteLine("Студента с таким ID нет");
    }//метод удаления студента для консоли, использует DeleteStudent из BusinessLogic

    /// <summary>
    /// Выводит гистограмму по специальностям символами #.
    /// </summary>
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