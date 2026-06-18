namespace Dede_Apteka.Dtos;

/// <summary>Тело запроса для /api/Auth/login.</summary>
public record LoginRequest(string Username, string Password);
