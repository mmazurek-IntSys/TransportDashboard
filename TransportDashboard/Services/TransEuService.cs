using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TransportDashboard.Services;

public class TransEuService
{
    private readonly HttpClient _http;

    public TransEuService(HttpClient http)
    {
        _http = http;
    }

    public async Task<Load[]> GetLoadsAsync()
    {
        var response = await _http.GetAsync("https://api.example.com/loads");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Load[]>(json) ?? Array.Empty<Load>();
    }
}

public class Load
{
    public string Id { get; set; } = "";
    public string PickupCity { get; set; } = "";
    public string DeliveryCity { get; set; } = "";
    public double WeightKg { get; set; }
    public string VehicleType { get; set; } = "";
    public decimal Price { get; set; }
}