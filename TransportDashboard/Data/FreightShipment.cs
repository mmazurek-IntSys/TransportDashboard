using System;
using System.Collections.Generic;
using System.Text;

namespace TransportDashboard.Models
{
    
        public class FreightShipment
        {
            public int Id { get; set; }
            public string FromCity { get; set; } = "";
            public string ToCity { get; set; } = "";
            public double WeightKg { get; set; }
            public double Price { get; set; }
        }
    
}
