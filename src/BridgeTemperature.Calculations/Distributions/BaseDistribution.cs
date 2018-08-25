using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Shared.Geometry;

namespace BridgeTemperature.Calculations.Distributions
{
    public abstract class BaseDistribution
    {
        private enum operationType { Addition, Subtraction };

        protected Interpolation interpolation;
        public IEnumerable<Distribution> Distribution { get; private set; }

        protected BaseDistribution(IEnumerable<Distribution> distribution)
        {
            this.UpdateDistribution(distribution);
        }

        private void UpdateDistribution(IEnumerable<Distribution> distribution)
        {
            var x = distribution.Select(e => e.Y);
            var y = distribution.Select(e => e.Value);
            this.interpolation = new Interpolation(x, y);
            this.Distribution = distribution;
        }

        public virtual double GetValue(double y)
        {
            return this.interpolation.Interpolate(y);
        }

        public void AddDistribution(IEnumerable<Distribution> distribution)
        {
            this.AddOrSubtract(distribution, operationType.Addition);
        }

        public void SubtractDistribution(IEnumerable<Distribution> distribution)
        {
            this.AddOrSubtract(distribution, operationType.Subtraction);
        }

        public void MultiplyDistribution(double value)
        {
            var multipliedDistribution = new List<Distribution>();
            foreach (var element in this.Distribution)
            {
                Distribution distribution = new Distribution();
                distribution.Value = element.Value * value;
                distribution.Y = element.Y;
                multipliedDistribution.Add(distribution);
            }
            UpdateDistribution(multipliedDistribution.Distinct().OrderBy(e => e.Y));
        }

        private void AddOrSubtract(IEnumerable<Distribution> distribution, operationType operationType)
        {
            var distributionSum = new List<Distribution>();
            var tempInterpolation = new Interpolation(distribution.Select(e => e.Y), distribution.Select(e => e.Value));
            int multiplier = (operationType == operationType.Addition) ? 1 : -1;
            foreach (var element in this.Distribution)
            {
                var newDistribution = new Distribution();
                newDistribution.Y = element.Y;
                newDistribution.Value = element.Value + tempInterpolation.Interpolate(element.Y) * multiplier;
                distributionSum.Add(newDistribution);
            }

            foreach (var element in distribution)
            {
                var newDistribution = new Distribution();
                newDistribution.Y = element.Y;
                newDistribution.Value = this.interpolation.Interpolate(element.Y) + element.Value * multiplier;
                distributionSum.Add(newDistribution);
            }

            UpdateDistribution(distributionSum.Distinct().OrderBy(e => e.Y));
        }
    }
}