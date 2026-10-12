using BL;

namespace TestData;

public static class TestStudents
{
    public static void AddTestStudents(BusinessLogic logic)
    {
        logic.AddStudent("Иван Иванов", "Программирование", "ПИ-21");
        logic.AddStudent("Алексей Петров", "Программирование", "ПИ-21");
        logic.AddStudent("Максим Сидоров", "Информационные системы", "ИС-22");
        logic.AddStudent("Анна Смирнова", "Экономика", "ЭК-21");
        logic.AddStudent("Дмитрий Кузнецов", "Информационные системы", "ИС-22");
        logic.AddStudent("Мария Попова", "Экономика", "ЭК-21");
        logic.AddStudent("Никита Волков", "Программирование", "ПИ-23");
        logic.AddStudent("Екатерина Соколова", "Информационные системы", "ИС-22");
        logic.AddStudent("Артём Морозов", "Программирование", "ПИ-23");
        logic.AddStudent("Ольга Новикова", "Экономика", "ЭК-21");
    }
}