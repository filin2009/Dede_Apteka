using System.Text;
using Dede_Apteka.Data;
using Dede_Apteka.Services;
using Dede_Apteka.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Не задана строка подключения ConnectionStrings:Default.");

// EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(connectionString));

// Контроллеры (перешли с minimal API на контроллеры)
builder.Services.AddControllers();

// FluentValidation — регистрируем все валидаторы из сборки
builder.Services.AddValidatorsFromAssemblyContaining<CreateReservationRequestValidator>();

// JWT-аутентификация
builder.Services.AddSingleton<JwtTokenService>();
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

// Прогон SQL-миграций Evolve при старте приложения
RunEvolveMigrations(connectionString, app.Logger);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Применяет все SQL-файлы из папки Migrations (embedded resources) к БД.
static void RunEvolveMigrations(string connectionString, ILogger logger)
{
    using var connection = new NpgsqlConnection(connectionString);
    var evolve = new EvolveDb.Evolve(connection, msg => logger.LogInformation("[Evolve] {Message}", msg))
    {
        EmbeddedResourceAssemblies = new[] { typeof(Program).Assembly },
        IsEraseDisabled = true
    };
    evolve.Migrate();
}
