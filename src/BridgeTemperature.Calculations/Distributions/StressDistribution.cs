using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Common.Geometry;

namespace BridgeTemperature.Calculations.Distributions
{
    public class StressDistribution : BaseDistribution
    {
        public StressDistribution(IEnumerable<Distribution> distribution) : base(distribution)
        {
        }

        public TemperatureDistribution ConvertToTemperatureDistribution(IEnumerable<PointD> coordinates, double modulusOfElasticity, double thermalCoefficient)
        {
            var temperatureDistribution = new List<Distribution>();

            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = base.GetValue(coordinate.Y) / thermalCoefficient / modulusOfElasticity;
                temperatureDistribution.Add(distribution);
            }
            foreach (var baseDistribution in base.Distribution)
            {
                var distribution = new Distribution();
                distribution.Y = baseDistribution.Y;
                distribution.Value = baseDistribution.Value / thermalCoefficient / modulusOfElasticity;
                temperatureDistribution.Add(distribution);
            }
            return new TemperatureDistribution(temperatureDistribution.Distinct().OrderBy(e => e.Y));
        }

        public static StressDistribution BendingStress(IEnumerable<PointD> coordinates, double bendingMoment, double centreOfGravity, double momentOfInertia, double baseModulusOfElasticity, double modulusOfElasticity)
        {
            var stressDistribution = new List<Distribution>();
            foreach (var coordinate in coordinates)
            {
                double alfa = baseModulusOfElasticity / modulusOfElasticity;
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = bendingMoment / momentOfInertia * (centreOfGravity - coordinate.Y) / alfa;
                stressDistribution.Add(distribution);
            }
            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }

        public static StressDistribution AxialStress(IEnumerable<PointD> coordinates, double axialForce, double area, double baseModulusOfElasticity, double modulusOfElasticity)
        {
            var stressDistribution = new List<Distribution>();

            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                var alfa = baseModulusOfElasticity / modulusOfElasticity;
                distribution.Y = coordinate.Y;
                distribution.Value = axialForce / area / alfa;
                stressDistribution.Add(distribution);
            }

            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }
    }
}