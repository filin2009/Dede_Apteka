namespace Dede_Apteka.Models;

/// <summary>
/// Бронь препарата на дату — сущность для «сложного» POST-запроса с валидацией даты.
/// </summary>
public class Reservation
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    /// <summary>Дата брони. Должна быть в диапазоне «сегодня ± 1 месяц» (проверяется FluentValidation).</summary>
    public DateTimeOffset ReservationDate { get; set; }
}
