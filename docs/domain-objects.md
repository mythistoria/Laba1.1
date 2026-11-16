# IDomainObject и два сценария DAL

## Текущая структура

- Единственный доменный класс — `Student`: `ID`, `Name`, `Speciality`, `Group`.
- `Student` реализует `Model.IDomainObject` со свойством `int ID { get; set; }`.
- `Context : DbContext` отображает студента на таблицу `Students`; свойство `ID` соответствует существующей колонке `Id`.
- `EntityRepository` и `DapperRepository` реализуют `IRepository<Student>` через EF Core и Dapper.
- BL хранит `IRepository<Student>` и выбирает реализацию по `StorageMode`.
- Исходники `View` и `WinFormsApp1` и их контракт с BL сохранены.

## Контракт

```csharp
public interface IRepository<T> where T : class, IDomainObject
{
    int Add(T item);
    bool Delete(int id);
    List<T> GetAll();
    T? GetById(int id);
    void Save(T item);
}
```

Ограничение задаётся один раз на уровне интерфейса. Даже объявление `IRepository<object>` или `IRepository<string>` не компилируется: тип должен быть ссылочным и реализовывать `IDomainObject`. У CRUD-методов нет собственных параметров типа и ограничений. Оба конкретных репозитория реализуют `IRepository<Student>`, поэтому принимают и возвращают студентов без приведения типов и проверки типа во время выполнения.

Обе реализации поддерживают `Student`. `Add` вставляет объект с ID = 0 и присваивает выданный базой ID. `Save` обновляет существующую запись с положительным ID. `GetAll` сортирует по ID, `GetById` возвращает `null` для отсутствующей записи, `Delete` сообщает факт удаления. Несовместимые типы отклоняются компилятором.

`IDomainObject` гарантирует наличие свойства, а генерацию идентификатора обеспечивает SQL Server.

```csharp
IRepository<Student> repository = new EntityRepository(); // либо new DapperRepository()
var student = new Student
{
    Name = "Иван Иванов",
    Speciality = "Программирование",
    Group = "ПИ-21"
};
repository.Add(student);
var saved = repository.GetById(student.ID)!;
saved.Group = "ПИ-22";
repository.Save(saved);
```

## Проверка

```sh
dotnet build Laba1.sln -p:EnableWindowsTargeting=true
```

Тестового проекта нет по решению пользователя. Настоящий запуск WinForms и SQL Server LocalDB требует Windows. Автоматическое удаление существующих таблиц базы не выполняется.
