using Dede_Apteka.Dtos;
using FluentValidation;

namespace Dede_Apteka.Validators;

/// <summary>
/// Валидатор «сложного» POST-запроса. Проверка вынесена из контроллера в отдельный
/// пакет FluentValidation (п.3.2 пятницы).
/// Условие: дата брони должна быть в диапазоне [сегодня - 1 месяц; сегодня + 1 месяц].
/// </summary>
public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Имя клиента обязательно.")
            .MaximumLength(100);

        RuleFor(x => x.ReservationDate)
            .Must(BeWithinOneMonth)
            .WithMessage(_ =>
            {
                var now = DateTimeOffset.UtcNow;
                return $"Дата должна быть в диапазоне от {now.AddMonths(-1):yyyy-MM-dd} " +
                       $"до {now.AddMonths(1):yyyy-MM-dd} (текущая дата ± 1 месяц).";
            });
    }

    private static bool BeWithinOneMonth(DateTimeOffset date)
    {
        var now = DateTimeOffset.UtcNow;
        return date >= now.AddMonths(-1) && date <= now.AddMonths(1);
    }
}
