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
    [TestFixture()]
    public class SectionPropertiesCalculationsTests
    {
        [Test()]
        public void SectionProperties_CalculateAllProperties_Passed()
        {
            var expectedResult = new Dictionary<SectionCharacteristic, double>();
            expectedResult.Add(SectionCharacteristic.Alfa, -1.25);
            expectedResult.Add(SectionCharacteristic.A, 400);
            expectedResult.Add(SectionCharacteristic.B, 22.19);//
            expectedResult.Add(SectionCharacteristic.H, 18.03);//
            expectedResult.Add(SectionCharacteristic.I1, 33333.33);
            expectedResult.Add(SectionCharacteristic.I2, 8333.33);
            expectedResult.Add(SectionCharacteristic.Ix, 73333.33);
            expectedResult.Add(SectionCharacteristic.Ix0, 10833.33);
            expectedResult.Add(SectionCharacteristic.Ixy, 70000);
            expectedResult.Add(SectionCharacteristic.Ixy0, 7500);
            expectedResult.Add(SectionCharacteristic.Iy, 93333.33);
            expectedResult.Add(SectionCharacteristic.Iy0, 30833.33);
            expectedResult.Add(SectionCharacteristic.Sx, 5000);//
            expectedResult.Add(SectionCharacteristic.Sy, 5000);//
            expectedResult.Add(SectionCharacteristic.X0, 12.5);
            expectedResult.Add(SectionCharacteristic.Y0, 12.5);
            expectedResult.Add(SectionCharacteristic.X0Max, 17.5);
            expectedResult.Add(SectionCharacteristic.X0Min, -12.5);
            expectedResult.Add(SectionCharacteristic.XIMax, 12.65);
            expectedResult.Add(SectionCharacteristic.XIMin, -15.81);
            expectedResult.Add(SectionCharacteristic.Y0Max, 7.5);
            expectedResult.Add(SectionCharacteristic.Y0Min, -12.5);
            expectedResult.Add(SectionCharacteristic.YIMax, 14.23);
            expectedResult.Add(SectionCharacteristic.YIMin, -17.39);
            expectedResult.Add(SectionCharacteristic.Xmax, 30);
            expectedResult.Add(SectionCharacteristic.Xmin, 0);
            expectedResult.Add(SectionCharacteristic.Ymax, 20);
            expectedResult.Add(SectionCharacteristic.Ymin, 0);

            IList<PointD> perimeter = new List<PointD>();
            perimeter.Add(new PointD(0, 0));
            perimeter.Add(new PointD(10, 0));
            perimeter.Add(new PointD(10, 10));
            perimeter.Add(new PointD(30, 10));
            perimeter.Add(new PointD(30, 20));
            perimeter.Add(new PointD(0, 20));
            SectionPropertiesCalculations calcs = new SectionPropertiesCalculations(perimeter);
            //calcs.CalculateProperties(perimeter);
            var actualResult = calcs.GetAllProperties();
            //rounding the result
            var actualResultRounded = new Dictionary<SectionCharacteristic, double>();
            foreach (KeyValuePair<SectionCharacteristic, double> value in actualResult)
            {
                double roundedValue = Math.Round(value.Value, 2);
                actualResultRounded.Add(value.Key, roundedValue);
            }


            CollectionAssert.AreEquivalent(expectedResult, actualResultRounded);
        }

    }
}