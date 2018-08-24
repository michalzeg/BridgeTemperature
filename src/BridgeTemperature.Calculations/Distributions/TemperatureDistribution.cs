using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.DistributionOperations
{
    public class TemperatureDistribution : BaseDistribution
    {
        public TemperatureDistribution(IEnumerable<Distribution> distribution) : base(distribution)
        {
        }

        public StressDistribution ConvertToStressDistribution(IEnumerable<PointD> coordinates, double modulusOfElasticity, double thermalCoefficient)
        {
            var stressDistribution = new List<Distribution>();
            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = this.interpolation.Interpolate(coordinate.Y) * thermalCoefficient * modulusOfElasticity;
                stressDistribution.Add(distribution);
            }
            foreach (var baseDistribution in base.Distribution)
            {
                var distribution = new Distribution();
                distribution.Y = baseDistribution.Y;
                distribution.Value = baseDistribution.Value * thermalCoefficient * modulusOfElasticity;
                stressDistribution.Add(distribution);
            }

            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }
    }
}