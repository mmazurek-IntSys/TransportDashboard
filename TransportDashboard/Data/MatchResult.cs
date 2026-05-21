using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Data
{
    public class MatchResult
    {
        public int ShipmentId { get; set; }
        public string VehicleId { get; set; } = "";
        public double Score { get; set; }
        public double Margin { get; set; }
    }
}