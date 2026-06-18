namespace Dede_Apteka.Models;

/// <summary>
/// Препарат в аптеке — основная сущность для CRUD.
/// </summary>
public class Drug
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
