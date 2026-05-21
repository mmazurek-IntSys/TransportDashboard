using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Models;

public class TransLoad
{
    public string Id { get; set; } = string.Empty;

    public string PickupCity { get; set; } = string.Empty;

    public string DeliveryCity { get; set; } = string.Empty;

    public double WeightKg { get; set; }

    public string VehicleType { get; set; } = string.Empty;

    public decimal Price { get; set; }
}