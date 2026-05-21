namespace TransportDashboard.Models;

public class TruckLocation
{
    public string VehicleId { get; set; } = string.Empty;

    public string DriverName { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double SpeedKmh { get; set; }

    public DateTime Timestamp { get; set; }
}