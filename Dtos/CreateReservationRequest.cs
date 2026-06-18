namespace Dede_Apteka.Dtos;

/// <summary>
/// «Сложный» POST-запрос с параметром-датой (п.3.2 пятницы).
/// Дата валидируется через FluentValidation: диапазон «сегодня ± 1 месяц».
/// Структура плоская, без вложенностей.
/// </summary>
public record CreateReservationRequest(string CustomerName, DateTimeOffset ReservationDate);
