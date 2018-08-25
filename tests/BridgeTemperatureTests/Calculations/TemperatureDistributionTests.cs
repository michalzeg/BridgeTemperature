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
    [TestFixture]
    public class TemperatureDistributionTests
    {
        [Test]
        public void ConvertToStressDistribution_Passed()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(10, 0),
                new PointD(10, 10),
                new PointD(0, 10),
                new PointD(0, 0)
            };

            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 5, Value = 5 }
            };

            var temperatureDistribution = new TemperatureDistribution(distribution);

            var expectedStressDitribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 240000 },
                new Distribution() { Y = 5, Value = 120000 },
                new Distribution() { Y = 10, Value = 0 }
            };

            var actualStressDistribution = temperatureDistribution.ConvertToStressDistribution(coordinates, 200000000, 0.00012);

            Assert.AreEqual(expectedStressDitribution, actualStressDistribution.Distribution);
        }
    }
}