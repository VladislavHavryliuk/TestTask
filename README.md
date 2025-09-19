# TestTask API

REST API на .NET 8 з використанням Entity Framework Core та JWT-аутентифікації.  
Реалізовано систему для керування користувачами та задачами цих користувачів з методами збереження,  редагування, читання відносно індентифікатора, читання всіх записів сутностей та видалення запису сутності.

## Стек розробки
- .NET 8 / ASP.NET Core Web API
- Entity Framework Core - ORM для роботи з БД PostgreSQL
- BCrypt.Net - для хешування паролів
- JWT - автентифікація та авторизація через JWT токени

## Алгоритм встановлення

1) Клонувати репозиторій
 
2) Налаштувати базу даних PostgreSQL:

Створіть БД **TestTaskDB** та оновіть рядок підключення в appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=TestTaskDB;Username=postgres;Password=YOUR_PASSWORD"
},
```
3) Виконайте наступні операції в Package Manager Console:
```bash
cd TestTask
dotnet ef database update
```
4) Запустіть проект)

## Аутентифікація

Реєстрація: POST /api/auth/register

Логін: POST /api/auth/login

Отриманий JWT додайте в **Authorization: Bearer <token>** для роботи з UserController та TaskController

## Структура проекту

- Controllers/ – контролери API

- Services/ – бізнес-логіка

- DTOs/ – DTO моделі

- Data/ – контекст EF Core

- Models/ – сутності БД




