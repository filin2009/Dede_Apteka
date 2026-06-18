using Dede_Apteka.Data;
using Dede_Apteka.Dtos;
using Dede_Apteka.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dede_Apteka.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly JwtTokenService _jwt;

    public AuthController(AppDbContext db, JwtTokenService jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    /// <summary>
    /// Аутентификация по логину/паролю. Возвращает JWT-токен.
    /// Сид-пользователь: admin / admin123.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var hash = JwtTokenService.HashPassword(request.Password);
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == hash);

        if (user is null)
            return Unauthorized(new { error = "Неверный логин или пароль." });

        return Ok(new { token = _jwt.GenerateToken(user) });
    }
}
