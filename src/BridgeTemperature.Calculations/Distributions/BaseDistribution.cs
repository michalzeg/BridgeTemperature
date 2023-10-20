using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Shared.Geometry;

namespace BridgeTemperature.Calculations.Distributions
{
    public abstract class BaseDistribution
    {
        private enum OperationType { Addition, Subtraction };

        protected Interpolation interpolation;
        public IEnumerable<Distribution> Distribution { get; private set; }

        protected BaseDistribution(IEnumerable<Distribution> distribution)
        {
            UpdateDistribution(distribution);
        }

        private void UpdateDistribution(IEnumerable<Distribution> distribution)
        {
            var x = distribution.Select(e => e.Y);
            var y = distribution.Select(e => e.Value);
            interpolation = new Interpolation(x, y);
            Distribution = distribution;
        }

        public virtual double GetValue(double y)
        {
            return interpolation.Interpolate(y);
        }

        public void AddDistribution(IEnumerable<Distribution> distribution)
        {
            AddOrSubtract(distribution, OperationType.Addition);
        }

        public void SubtractDistribution(IEnumerable<Distribution> distribution)
        {
            AddOrSubtract(distribution, OperationType.Subtraction);
        }

        public void MultiplyDistribution(double value)
        {
            var multipliedDistribution = new List<Distribution>();
            foreach (var element in Distribution)
            {
                Distribution distribution = new Distribution
                {
                    Value = element.Value * value,
                    Y = element.Y
                };
                multipliedDistribution.Add(distribution);
            }
            UpdateDistribution(multipliedDistribution.Distinct().OrderBy(e => e.Y));
        }

        private void AddOrSubtract(IEnumerable<Distribution> distribution, OperationType operationType)
        {
            var distributionSum = new List<Distribution>();
            var tempInterpolation = new Interpolation(distribution.Select(e => e.Y), distribution.Select(e => e.Value));
            int multiplier = (operationType == OperationType.Addition) ? 1 : -1;
            foreach (var element in Distribution)
            {
                var newDistribution = new Distribution
                {
                    Y = element.Y,
                    Value = element.Value + tempInterpolation.Interpolate(element.Y) * multiplier
                };
                distributionSum.Add(newDistribution);
            }

            foreach (var element in distribution)
            {
                var newDistribution = new Distribution
                {
                    Y = element.Y,
                    Value = interpolation.Interpolate(element.Y) + element.Value * multiplier
                };
                distributionSum.Add(newDistribution);
            }

            UpdateDistribution(distributionSum.Distinct().OrderBy(e => e.Y));
        }
    }
}