using TransportDashboard.Data;
using TransportDashboard.AI;
using TransportDashboard.Models;
using FreightShipment = TransportDashboard.Models.FreightShipment;

namespace TransportDashboard.AI
{
    public class MatchingEngine
    {
        private readonly ScoringService _scoring = new();

        public MatchResult? Match(FreightShipment s, List<Vehicle> vehicles)
        {
            var best = vehicles
                .Where(v => v.IsFree && v.CapacityKg >= s.WeightKg)
                .OrderByDescending(v => _scoring.Calculate(s, v))
                .FirstOrDefault();

            if (best == null) return null;

            double score = _scoring.Calculate(s, best);
            double cost = s.WeightKg * 1.4;
            double margin = s.Price - cost;

            best.IsFree = false;

            return new MatchResult
            {
                ShipmentId = s.Id,
                VehicleId = best.Id,
                Score = score,
                Margin = margin
            };
        }
    }
}