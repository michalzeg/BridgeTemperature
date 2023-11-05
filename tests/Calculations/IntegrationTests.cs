using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Interpolation;
using System.Threading.Tasks;
using NSubstitute;
using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Shared.Sections;
using BridgeTemperature.Calculations.Calculators;

namespace BridgeTemperatureTests.Calculations
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void IntegrateTest_RectangleFunctionConstant_Passed()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(10, 0),
                new PointD(10, 10),
                new PointD(0, 10),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();
            section.CentreOfGravity.Returns(new PointD(5, 5));
            section.Coordinates.Returns(coordinates);
            section.Type.Returns(SectionType.Steel);
            section.YMax.Returns(10);
            section.YMin.Returns(0);

            var interigation = new Integration();

            interigation.Integrate(section, section.CentreOfGravity, (e) => 1);

            Assert.AreEqual(100, interigation.NormalForce, 0.000001);
            Assert.AreEqual(0, interigation.Moment, 0.0000001);
        }

        [Test]
        public void IntegrateTest_TraingleFunctionConstant_Passed()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(9, 0),
                new PointD(0, 9),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();

            section.CentreOfGravity.Returns(new PointD(5, 9 / 3));
            section.Coordinates.Returns(coordinates);
            section.Type.Returns(SectionType.Steel);
            section.YMax.Returns(9);
            section.YMin.Returns(0);

            var interigation = new Integration();
            interigation.Integrate(section, section.CentreOfGravity, (e) => 2);

            Assert.AreEqual(2 * 40.5, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(0, interigation.Moment, 0.0000000001);
        }

        [Test()]
        public void IntegrateTest_RectangleFunctionLinear_Passed()
        {
            IList<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(10, 0),
                new PointD(10, 10),
                new PointD(0, 10),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();

            section.CentreOfGravity.Returns(new PointD(5, 5));
            section.Coordinates.Returns(coordinates);
            section.Type.Returns(SectionType.Steel);
            section.YMax.Returns(10);
            section.YMin.Returns(0);

            var integrationFunction = LinearSpline.InterpolateSorted(new double[] { 0, 10 }, new double[] { 6, -6 });

            var interigation = new Integration();
            interigation.Integrate(section, section.CentreOfGravity, integrationFunction.Interpolate);

            Assert.AreEqual(0, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(1000, interigation.Moment, 0.001);
        }

        [Test]
        public void IntegrateTest_RectangleFunctionLinearNotEqualValues_Passed()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(10, 0),
                new PointD(10, 10),
                new PointD(0, 10),
                new PointD(0, 0)
            };

            var section = Substitute.For<IIntegrable>();

            section.CentreOfGravity.Returns(new PointD(5, 5));
            section.Coordinates.Returns(coordinates);
            section.Type.Returns(SectionType.Steel);
            section.YMax.Returns(10);
            section.YMin.Returns(0);

            var integrationFunction = LinearSpline.InterpolateSorted(new double[] { 0, 10 }, new double[] { -3, -6 });

            var interigation = new Integration();
            interigation.Integrate(section, section.CentreOfGravity, integrationFunction.Interpolate);

            Assert.AreEqual(-450, interigation.NormalForce, 0.001);
            Assert.AreEqual(250, interigation.Moment, 0.001);
        }
    }
}