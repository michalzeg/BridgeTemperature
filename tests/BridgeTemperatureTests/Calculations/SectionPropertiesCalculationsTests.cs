using NUnit.Framework;
using BridgeTemperature.SectionProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.SectionProperties.Tests
{
    [TestFixture]
    public class SectionPropertiesCalculationsTests
    {
        [Test]
        public void SectionProperties_CalculateAllProperties_Passed()
        {
            var expectedResult = new Dictionary<SectionCharacteristic, double>
            {
                { SectionCharacteristic.Alfa, -1.25 },
                { SectionCharacteristic.A, 400 },
                { SectionCharacteristic.B, 22.19 },
                { SectionCharacteristic.H, 18.03 },
                { SectionCharacteristic.I1, 33333.33 },
                { SectionCharacteristic.I2, 8333.33 },
                { SectionCharacteristic.Ix, 73333.33 },
                { SectionCharacteristic.Ix0, 10833.33 },
                { SectionCharacteristic.Ixy, 70000 },
                { SectionCharacteristic.Ixy0, 7500 },
                { SectionCharacteristic.Iy, 93333.33 },
                { SectionCharacteristic.Iy0, 30833.33 },
                { SectionCharacteristic.Sx, 5000 },
                { SectionCharacteristic.Sy, 5000 },
                { SectionCharacteristic.X0, 12.5 },
                { SectionCharacteristic.Y0, 12.5 },
                { SectionCharacteristic.X0Max, 17.5 },
                { SectionCharacteristic.X0Min, -12.5 },
                { SectionCharacteristic.XIMax, 12.65 },
                { SectionCharacteristic.XIMin, -15.81 },
                { SectionCharacteristic.Y0Max, 7.5 },
                { SectionCharacteristic.Y0Min, -12.5 },
                { SectionCharacteristic.YIMax, 14.23 },
                { SectionCharacteristic.YIMin, -17.39 },
                { SectionCharacteristic.Xmax, 30 },
                { SectionCharacteristic.Xmin, 0 },
                { SectionCharacteristic.Ymax, 20 },
                { SectionCharacteristic.Ymin, 0 }
            };

            IList<PointD> perimeter = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(10, 0),
                new PointD(10, 10),
                new PointD(30, 10),
                new PointD(30, 20),
                new PointD(0, 20)
            };
            var calculations = new SectionPropertiesCalculations(perimeter);

            var actualResult = calculations
                                .GetAllProperties()
                                .Select(e => new { Key = e.Key, Value = Math.Round(e.Value, 2) })
                                .ToDictionary(e => e.Key, f => f.Value);

            CollectionAssert.AreEquivalent(expectedResult, actualResult);
        }
    }
}