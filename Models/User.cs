namespace Dede_Apteka.Models;

/// <summary>
/// Пользователь системы — нужен для аутентификации (выдачи JWT-токена).
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    /// <summary>SHA-256 хэш пароля в hex.</summary>
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "user";
}
