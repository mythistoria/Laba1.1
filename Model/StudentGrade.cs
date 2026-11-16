namespace Model;

/// <summary>Оценка по предмету — часть студента в композиции 1:m.</summary>
public class StudentGrade : IDomainObject
{
    public int ID { get; set; }
    public string Subject { get; set; } = "";
    public int Value { get; set; }
    public int StudentID { get; set; }
    public Student Student { get; set; } = null!;
}
