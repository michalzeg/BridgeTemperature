using NUnit.Framework;
using BridgeTemperature.DistributionOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.DistributionOperations.Tests
{
    [TestFixture()]
    public class TemperatureDistributionTests
    {
        [Test()]
        public void ConvertToStressDistribution_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 10));
            coordinates.Add(new PointD(0, 10));
            coordinates.Add(new PointD(0, 0));

            var distribution = new List<Distribution>();
            distribution.Add(new Distribution() { Y = 0, Value = 10 });
            distribution.Add(new Distribution() { Y = 5, Value = 5 });

            var temperatureDistribution = new TemperatureDistribution(distribution);

            var expectedStressDitribution = new List<Distribution>();
            expectedStressDitribution.Add(new Distribution() { Y = 0, Value = 240000 });
            expectedStressDitribution.Add(new Distribution() { Y = 5, Value = 120000 });
            expectedStressDitribution.Add(new Distribution() { Y = 10, Value = 0 });

            var actualStressDistribution = temperatureDistribution.ConvertToStressDistribution(coordinates, 200000000, 0.00012);
            Assert.AreEqual(expectedStressDitribution, actualStressDistribution.Distribution);
        }
    }
}