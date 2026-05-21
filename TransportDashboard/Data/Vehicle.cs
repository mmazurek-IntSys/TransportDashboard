using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace TransportDashboard.Data
{
    public class Vehicle
    {
        public string Id { get; set; } = "";
        public double X { get; set; }
        public double Y { get; set; }
        public bool IsFree { get; set; } = true;
        public double CapacityKg { get; set; }
    }
}