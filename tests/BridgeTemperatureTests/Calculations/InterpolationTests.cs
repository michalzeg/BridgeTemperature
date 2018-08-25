using BridgeTemperature.Shared.Geometry;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTemperatureTests.Calculations
{
    [TestFixture()]
    public class InterpolationTests
    {
        [TestCase(0, 0)]
        [TestCase(10, 10)]
        [TestCase(-2, 0)]
        [TestCase(19, 10)]
        [TestCase(30, 20)]
        [TestCase(5, 5)]
        [TestCase(25, 15)]
        public void InterpolateTest_Passed(double value, double expectedValue)
        {
            var x = new double[] { 0, 10, 20, 20, 30 };
            var y = new double[] { 0, 10, 10, 10, 20 };

            var interpolation = new Interpolation(x, y);

            var actualValue = interpolation.Interpolate(value);

            Assert.AreEqual(expectedValue, actualValue, 0.001);
        }
    }
}