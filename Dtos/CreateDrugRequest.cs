namespace Dede_Apteka.Dtos;

/// <summary>
/// Запрос на создание препарата (CRUD-POST).
/// Только обязательные параметры, без вложенных структур (п.1 пятницы).
/// </summary>
public record CreateDrugRequest(string Name, string Manufacturer, decimal Price, int Quantity);
