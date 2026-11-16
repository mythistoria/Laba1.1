namespace Model;

/// <summary>
/// Доменный объект: студент. Хранится в таблице Students.
/// </summary>
public class Student : IDomainObject
{
    /// <summary>
    /// Первичный ключ. Значение выдаёт база данных (IDENTITY), вручную не задаётся.
    /// </summary>
    public int ID { get; set; }
    /// <summary>
    /// ФИО студента.
    /// </summary>
    public string Name { get; set; } = "";
    /// <summary>
    /// Специальность.
    /// </summary>
    public string Speciality { get; set; } = "";
    /// <summary>
    /// Учебная группа (в таблице колонка называется GroupName, т.к. GROUP - служебное слово SQL).
    /// </summary>
    public string Group { get; set; } = "";

}
