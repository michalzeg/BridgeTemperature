using BridgeTemperature.Calculations.Distributions;
using BridgeTemperature.Calculations.Sections;
using BridgeTemperature.Common.Geometry;
using BridgeTemperature.Common.Sections;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTemperatureTests.Calculations
{
    [TestFixture()]
    public class SectionTests
    {
        [Test()]
        public void SectionTest()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(0, 1),
                new PointD(12, 1),
                new PointD(12, 0)
            };

            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 10, Value = 10 }
            };

            var section = new Section(coordinates, SectionType.Steel, 200000000, 0.00001, distribution);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(12d, section.Area);
                Assert.AreEqual(1d, section.MomentOfInertia);
                Assert.AreEqual(6, section.CentreOfGravity.X);
                Assert.AreEqual(0.5, section.CentreOfGravity.Y);
                Assert.AreEqual(0, section.YMin);
                Assert.AreEqual(1, section.YMax);
                Assert.AreEqual(1, section.Height);
            });
        }
    }
}