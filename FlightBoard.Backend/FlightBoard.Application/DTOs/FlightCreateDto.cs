namespace FlightBoard.Application.DTOs;

public class FlightCreateDto
{
    public string FlightNumber { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Gate { get; set; } = string.Empty;
    public DateTime DepartureTime { get; set; }
}
