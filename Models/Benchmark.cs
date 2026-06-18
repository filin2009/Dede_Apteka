namespace Dede_Apteka.Models;

/// <summary>
/// Таблица для замеров производительности вставки.
/// insert_time хранится как timestamp with timezone (PostgreSQL timestamptz).
/// </summary>
public class Benchmark
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset InsertTime { get; set; }
}
