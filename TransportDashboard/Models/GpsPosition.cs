namespace TransportDashboard.Models;

public class GpsPosition
{
    public long Id { get; set; }

    public long DeviceId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double Speed { get; set; }

    public double Course { get; set; }

    public DateTime DeviceTime { get; set; }

    public string Address { get; set; } = string.Empty;
}