# IDomainObject и два сценария DAL

## Текущая структура

- Единственный доменный класс — `Student`: `ID`, `Name`, `Speciality`, `Group`.
- `Student` реализует `Model.IDomainObject` со свойством `int ID { get; set; }`.
- `Context : DbContext` отображает студента на таблицу `Students`; свойство `ID` соответствует существующей колонке `Id`.
- `EntityRepository` и `DapperRepository` реализуют общий `IRepository` через EF Core и Dapper.
- BL хранит `IRepository` и выбирает реализацию по `StorageMode`.
- Исходники `View` и `WinFormsApp1` и их контракт с BL сохранены.

## Контракт

```csharp
int Add<T>(T item) where T : class, IDomainObject;
bool Delete<T>(int id) where T : class, IDomainObject;
List<T> GetAll<T>() where T : class, IDomainObject;
T? GetById<T>(int id) where T : class, IDomainObject;
void Save<T>(T item) where T : class, IDomainObject;
```

Все методы ограничивают тип `T` условием `class, IDomainObject`: компилятор требует ссылочный тип, реализующий контракт со свойством `ID`. Например, `repository.Add(new object())` и `repository.GetAll<string>()` не компилируются. Реализации повторяют ограничения интерфейса.

Обе реализации поддерживают `Student`. `Add` вставляет объект с ID = 0 и присваивает выданный базой ID. `Save` обновляет существующую запись с положительным ID. `GetAll` сортирует по ID, `GetById` возвращает `null` для отсутствующей записи, `Delete` сообщает факт удаления. Типы, не включённые в модель хранения, отклоняются.

`IDomainObject` гарантирует наличие свойства, а генерацию идентификатора обеспечивает SQL Server.

```csharp
IRepository repository = new EntityRepository(); // либо new DapperRepository()
var student = new Student
{
    Name = "Иван Иванов",
    Speciality = "Программирование",
    Group = "ПИ-21"
};
repository.Add(student);
var saved = repository.GetById<Student>(student.ID)!;
saved.Group = "ПИ-22";
repository.Save(saved);
```

## Проверка

```sh
dotnet build Laba1.sln -p:EnableWindowsTargeting=true
```

Тестового проекта нет по решению пользователя. Настоящий запуск WinForms и SQL Server LocalDB требует Windows. Автоматическое удаление существующих таблиц базы не выполняется.
