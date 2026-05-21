using TransportDashboard.AI;
using TransportDashboard.Data;
using TransportDashboard.Models;

namespace TransportDashboard.Services
{
    public class DispatchService
    {
        private readonly MatchingEngine _engine = new();

        public List<MatchResult> Run(List<FreightShipment> shipments, List<Vehicle> vehicles)
        {
            var results = new List<MatchResult>();

            foreach (var s in shipments)
            {
                var match = _engine.Match(s, vehicles);
                if (match != null)
                    results.Add(match);
            }

            return results;
        }
    }
}
