using Dede_Apteka.Data;
using Dede_Apteka.Dtos;
using Dede_Apteka.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dede_Apteka.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrugsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DrugsController(AppDbContext db) => _db = db;

    /// <summary>Список препаратов. Открыт без токена.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Drugs.ToListAsync());

    /// <summary>Препарат по id. Открыт без токена.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var drug = await _db.Drugs.FindAsync(id);
        return drug is null ? NotFound() : Ok(drug);
    }

    /// <summary>
    /// Создание препарата (CRUD-POST). Параметры плоские и обязательные.
    /// Закрыт за JWT-токеном.
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDrugRequest request)
    {
        var drug = new Drug
        {
            Name = request.Name,
            Manufacturer = request.Manufacturer,
            Price = request.Price,
            Quantity = request.Quantity
        };
        _db.Drugs.Add(drug);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = drug.Id }, drug);
    }

    /// <summary>Обновление препарата. Закрыт за JWT-токеном.</summary>
    [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateDrugRequest request)
    {
        var drug = await _db.Drugs.FindAsync(id);
        if (drug is null) return NotFound();

        drug.Name = request.Name;
        drug.Manufacturer = request.Manufacturer;
        drug.Price = request.Price;
        drug.Quantity = request.Quantity;
        await _db.SaveChangesAsync();
        return Ok(drug);
    }

    /// <summary>Удаление препарата. Закрыт за JWT-токеном.</summary>
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var drug = await _db.Drugs.FindAsync(id);
        if (drug is null) return NotFound();

        _db.Drugs.Remove(drug);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
