using Dede_Apteka.Data;
using Dede_Apteka.Dtos;
using Dede_Apteka.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Dede_Apteka.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IValidator<CreateReservationRequest> _validator;

    public ReservationsController(AppDbContext db, IValidator<CreateReservationRequest> validator)
    {
        _db = db;
        _validator = validator;
    }

    /// <summary>
    /// «Сложный» POST: создание брони с параметром-датой.
    /// Валидация даты (диапазон «сегодня ± 1 месяц») выполняется НЕ в контроллере,
    /// а отдельным пакетом FluentValidation. При ошибке — 400 с сообщением.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationRequest request)
    {
        var result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
            return BadRequest(new { errors = result.Errors.Select(e => e.ErrorMessage) });

        var reservation = new Reservation
        {
            CustomerName = request.CustomerName,
            ReservationDate = request.ReservationDate
        };
        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        return Ok(reservation);
    }
}
