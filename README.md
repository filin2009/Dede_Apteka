# Dede_Apteka

Учебный ASP.NET Core (.NET 10) проект по теме «Аптека».
Стек: **EF Core 10 + Npgsql (PostgreSQL)**, миграции через **Evolve** (SQL-файлы),
аутентификация **JWT**, валидация **FluentValidation**.

Автор задания: **Dede**.

---

## Ответы на вопросы (readme по заданию)

### 1. Как называются сложные эндпоинты (куда смотреть)
«Сложный» эндпоинт — `POST /api/Reservations`
([Controllers/ReservationsController.cs](Controllers/ReservationsController.cs)).
Это POST с параметром-датой, который валидируется отдельным пакетом **FluentValidation**
(правило — в [Validators/CreateReservationRequestValidator.cs](Validators/CreateReservationRequestValidator.cs)):
дата брони должна быть в диапазоне **сегодня ± 1 месяц**, иначе возвращается 400 с пояснением.

### 2. Как получить токен (логин/пароль для /api/Auth/login)
Запрос:
```http
POST /api/Auth/login
Content-Type: application/json

{ "username": "admin", "password": "admin123" }
```
В ответе — `{ "token": "<JWT>" }`. Дальше токен передаётся в заголовке:
`Authorization: Bearer <JWT>`.

Логин/пароль: **admin / admin123**. Пользователь засеян SQL-миграцией
([Migrations/V1__init.sql](Migrations/V1__init.sql)); пароль хранится как SHA-256 хэш.

### 3. Какие эндпоинты закрыты за токеном
Атрибутом `[Authorize]` закрыты операции изменения препаратов
([Controllers/DrugsController.cs](Controllers/DrugsController.cs)):

| Метод | Эндпоинт            | Требует токен |
|-------|---------------------|:-------------:|
| POST  | `/api/Drugs`        | ✅ |
| PUT   | `/api/Drugs/{id}`   | ✅ |
| DELETE| `/api/Drugs/{id}`   | ✅ |
| GET   | `/api/Drugs`        | ❌ |
| GET   | `/api/Drugs/{id}`   | ❌ |
| GET   | `/api/Benchmark/insert` | ❌ |
| POST  | `/api/Reservations` | ❌ |
| POST  | `/api/Auth/login`   | ❌ |

### 4. Какой пакет использовали для миграций
**Evolve** (3.2.0). Изменения БД хранятся как чистые SQL-файлы в папке
[Migrations/](Migrations/) и подключены как embedded resources (см. `.csproj`).
Evolve прогоняет их при старте приложения (`RunEvolveMigrations` в [Program.cs](Program.cs)).

---

## Структура и эндпоинты

- `POST /api/Auth/login` — получить JWT.
- `GET/POST/PUT/DELETE /api/Drugs` — CRUD по препаратам (запись — под токеном).
- `GET /api/Benchmark/insert?message=...` — вставка строки в таблицу `benchmark`
  для замеров. Вызывается прямо из браузера (параметр в URL).
- `POST /api/Reservations` — «сложный» POST с валидацией даты (FluentValidation).

Таблицы БД: `drugs`, `users`, `benchmark`, `dede`, `reservations`.

---

## Запуск

1. Создать БД в PostgreSQL (например `dede_apteka`).
2. Прописать строку подключения в [appsettings.json](appsettings.json) → `ConnectionStrings:Default`.
3. `dotnet run` — Evolve создаст таблицы и засеет пользователя `admin`.
4. Примеры запросов — в [Dede_Apteka.http](Dede_Apteka.http).

---

## Теория (конспект по заданию)

### Что такое миграции и зачем нужны
Миграция — версионированное изменение структуры БД (создать таблицу, добавить колонку
и т.п.). Нужны, чтобы изменения схемы воспроизводились на всех окружениях
(dev/test/prod) детерминированно, хранились в системе контроля версий рядом с кодом,
накатывались инкрементально и не терялись между разработчиками.

### Миграции в EF Core
EF Core хранит миграции как C#-классы (`Up()`/`Down()`) + снапшот модели.
Основные команды CLI (`dotnet ef`):
- `dotnet ef migrations add <Name>` — сгенерировать файл миграции из изменений модели;
- `dotnet ef database update` — применить миграции к БД;
- `dotnet ef migrations script` — выгрузить миграции в SQL;
- `dotnet ef migrations remove` — удалить последнюю (непринятую) миграцию.
Состояние накатанных миграций EF хранит в таблице `__EFMigrationsHistory`.
В EF Core 10 миграции по-прежнему C#-классы; среди новшеств — улучшения LINQ/SQL
и поддержки провайдеров.

### Почему здесь Evolve, а не EF Migrations
По заданию изменения БД должны лежать в **SQL-файлах**. EF-миграции — это C#.
Evolve (как Flyway) применяет именно `.sql`-файлы с версией в имени
(`V1__init.sql`, `V2__...`), отслеживая их в служебной таблице `changelog`.

### Валидация (п.3.1 пятницы)
Валидация — проверка входных данных до использования (типы, диапазоны, обязательность).
- **Встроенно в EF Core / ASP.NET**: DataAnnotations (`[Required]`, `[Range]`,
  `[MaxLength]`), `ModelState.IsValid`; на уровне модели EF — ограничения столбцов.
  В EF Core 10 усилены возможности по ограничениям/маппингу.
- **Сторонние пакеты**: **FluentValidation** (используется здесь), а также
  валидаторы внутри MediatR-пайплайнов и др.
Здесь проверка даты вынесена из контроллера в отдельный класс-валидатор FluentValidation.
