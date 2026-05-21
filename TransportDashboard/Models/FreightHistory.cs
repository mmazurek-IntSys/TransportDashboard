using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Models;

public class FreightHistory
{
    public int Id { get; set; }

    public string LoadId { get; set; } = string.Empty;

    public string PickupCity { get; set; } = string.Empty;

    public string DeliveryCity { get; set; } = string.Empty;

    public double WeightKg { get; set; }

    public string VehicleType { get; set; } = string.Empty;

    public decimal OfferedPrice { get; set; }

    public decimal CalculatedMargin { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public int ClientId { get; set; }

    public Client Client { get; set; } = null!;
}
