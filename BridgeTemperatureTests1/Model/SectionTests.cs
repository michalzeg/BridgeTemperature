using NUnit.Framework;
using BridgeTemperature.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Helpers;
using BridgeTemperature.DistributionOperations;

namespace BridgeTemperature.Sections.Tests
{
    [TestFixture()]
    public class SectionTests
    {
        [Test()]
        public void SectionTest()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(0, 1));
            coordinates.Add(new PointD(12, 1));
            coordinates.Add(new PointD(12, 0));

            var distribution = new List<Distribution>();
            distribution.Add(new Distribution() { Y = 0, Value = 10 });
            distribution.Add(new Distribution() { Y = 10, Value = 10 });

            ISection section = new Section(coordinates, SectionType.Fill, 200000000, 0.00001,distribution);

            Assert.AreEqual(12d, section.Area);
            Assert.AreEqual(1d, section.MomentOfInertia);
            Assert.AreEqual(6, section.CentreOfGravity.X);
            Assert.AreEqual(0.5, section.CentreOfGravity.Y);
            Assert.AreEqual(0, section.YMin);
            Assert.AreEqual(1, section.YMax);
            Assert.AreEqual(1, section.Height);
            
        }
    }
}