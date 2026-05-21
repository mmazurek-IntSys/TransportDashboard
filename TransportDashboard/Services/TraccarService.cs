using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TransportDashboard.Services;

public class TraccarService
{
    private readonly HttpClient _http;

    public TraccarService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Position[]> GetPositionsAsync()
    {
        // Przykład: pobranie danych z API
        var response = await _http.GetAsync("https://api.example.com/positions");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Position[]>(json) ?? Array.Empty<Position>();
    }
}

// Klasa reprezentująca pojazd
public class Position
{
    public string DeviceId { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public DateTime DeviceTime { get; set; }
    public string Address { get; set; } = "";
}