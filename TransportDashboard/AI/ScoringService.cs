using TransportDashboard.Data;
using TransportDashboard.Models;


namespace TransportDashboard.AI
{
    public class ScoringService
    {
        public double Calculate(FreightShipment s, Vehicle v)
        {
            double distance = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            double cost = s.WeightKg * 1.4;

            double margin = ((s.Price - cost) / cost) * 100;
            double capacityBonus = v.CapacityKg >= s.WeightKg ? 10 : -50;
            double distancePenalty = distance * 0.05;

            return 50 + margin * 1.5 + capacityBonus - distancePenalty;
        }

        
    }
}