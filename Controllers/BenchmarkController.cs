using System.Diagnostics;
using Dede_Apteka.Data;
using Dede_Apteka.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dede_Apteka.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BenchmarkController : ControllerBase
{
    private readonly AppDbContext _db;

    public BenchmarkController(AppDbContext db) => _db = db;

    /// <summary>
    /// Вставка строки в таблицу benchmark для замеров производительности.
    /// GET, чтобы вызывать прямо из браузера: /api/Benchmark/insert?message=hello
    /// Единственный параметр — message (передаётся через URL). Открыт без токена.
    /// </summary>
    [HttpGet("insert")]
    public async Task<IActionResult> Insert([FromQuery] string message)
    {
        var sw = Stopwatch.StartNew();

        var row = new Benchmark
        {
            Message = message,
            InsertTime = DateTimeOffset.UtcNow
        };
        _db.Benchmarks.Add(row);
        await _db.SaveChangesAsync();

        sw.Stop();
        return Ok(new
        {
            id = row.Id,
            message = row.Message,
            insertTime = row.InsertTime,
            elapsedMs = sw.Elapsed.TotalMilliseconds
        });
    }
}
