using NUnit.Framework;
using BridgeTemperature.IntegrationFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Interpolation;
using System.Threading.Tasks;
using NSubstitute;
using BridgeTemperature.Sections;
using BridgeTemperature.Helpers;
using BridgeTemperature.Extensions;
namespace BridgeTemperature.IntegrationFunctions.Tests
{
    [TestFixture()]
    public class IntegrationTests
    {

        [Test()]
        public void IntegrateTest_RectangleFunctionConstant_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 10));
            coordinates.Add(new PointD(0, 10));
            coordinates.Add(new PointD(0, 0));

            var section = Substitute.For<IIntegrable>();
            //section.Area.Returns(100);
            section.CentreOfGravity.Returns(new PointD(5, 5));
            //section.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            section.Coordinates.Returns(coordinates);
            //section.ModulusOfElasticity.Returns(200000000);
            //section.ThermalCooeficient.Returns(0.00001);
            section.Type.Returns(SectionType.Fill);
            section.YMax.Returns(10);
            section.YMin.Returns(0);


            //var compositeSection = Substitute.For<IIntegrable>();
            //compositeSection.CentreOfGravity.Returns(new PointD(5, 5));
            //compositeSection.Height.Returns(10);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12);
            //compositeSection.Sections.Returns(new List<ISection>() { section });

            Integration interigation = new Integration();
            interigation.Integrate(section, (e) => 1);


            Assert.AreEqual(100, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(0, interigation.Moment, 0.0000000001);
        }
        [Test()]
        public void IntegrateTest_TraingleFunctionConstant_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(9, 0));
            coordinates.Add(new PointD(0, 9));
            coordinates.Add(new PointD(0, 0));

            var section = Substitute.For<IIntegrable>();
            //section.Area.Returns(9 * 9 / 2);
            section.CentreOfGravity.Returns(new PointD(5, 9 / 3));
            //section.MomentOfInertia.Returns(9 * 9 * 9 * 9 / 36);
            section.Coordinates.Returns(coordinates);
            //section.ModulusOfElasticity.Returns(200000000);
            //section.ThermalCooeficient.Returns(0.00001);
            section.Type.Returns(SectionType.Fill);
            section.YMax.Returns(9);
            section.YMin.Returns(0);


            //var compositeSection = Substitute.For<ICompositeSection>();
            //compositeSection.CentreOfGravity.Returns(new PointD(0, 3));
            //compositeSection.Height.Returns(9);
            //compositeSection.MomentOfIntertia.Returns(9 * 9 * 9 * 9 / 36);
            //compositeSection.Sections.Returns(new List<ISection>() { section });

            Integration interigation = new Integration();
            interigation.Integrate(section, (e) => 2);


            Assert.AreEqual(2 * 40.5, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(0, interigation.Moment, 0.0000000001);
        }

        [Test()]
        public void IntegrateTest_RectangleFunctionLinear_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 10));
            coordinates.Add(new PointD(0, 10));
            coordinates.Add(new PointD(0, 0));

            var section = Substitute.For<IIntegrable>();
            //section.Area.Returns(100);
            section.CentreOfGravity.Returns(new PointD(5, 5));
            //section.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            section.Coordinates.Returns(coordinates);
            //section.ModulusOfElasticity.Returns(200000000);
            //section.ThermalCooeficient.Returns(0.00001);
            section.Type.Returns(SectionType.Fill);
            section.YMax.Returns(10);
            section.YMin.Returns(0);


            //var compositeSection = Substitute.For<IIntegrable>();
            //compositeSection.CentreOfGravity.Returns(new PointD(5, 5));
            //compositeSection.Height.Returns(10);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12);
            //compositeSection.Sections.Returns(new List<ISection>() { section });

            var integrationFunction = LinearSpline.InterpolateSorted(new double[] { 0, 10 }, new double[] { 6, -6 });
            Integration interigation = new Integration();
            interigation.Integrate(section, integrationFunction.Interpolate);


            Assert.AreEqual(0, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(1000, interigation.Moment, 0.001);

            

        }
        [Test()]
        public void IntegrateTest_RectangleFunctionLinearNotEqualValues_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 10));
            coordinates.Add(new PointD(0, 10));
            coordinates.Add(new PointD(0, 0));

            var section = Substitute.For<IIntegrable>();
            //section.Area.Returns(100);
            section.CentreOfGravity.Returns(new PointD(5, 5));
            //section.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            section.Coordinates.Returns(coordinates);
            //section.ModulusOfElasticity.Returns(200000000);
            ///section.ThermalCooeficient.Returns(0.00001);
            section.Type.Returns(SectionType.Fill);
            section.YMax.Returns(10);
            section.YMin.Returns(0);


            //var compositeSection = Substitute.For<IIntegrable>();
            //compositeSection.CentreOfGravity.Returns(new PointD(5, 5));
            //compositeSection.Height.Returns(10);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12);
            //compositeSection.Sections.Returns(new List<ISection>() { section });

            var integrationFunction = LinearSpline.InterpolateSorted(new double[] { 0, 10 }, new double[] { -3, -6 });
            Integration interigation = new Integration();
            interigation.Integrate(section, integrationFunction.Interpolate);


            Assert.AreEqual(-450, interigation.NormalForce, 0.001);
            Assert.AreEqual(250, interigation.Moment, 0.001);



        }
        /*[Test()]
        public void IntegrateTest_HollowSectionFunctionConstant_Passed()
        {
            IList<PointD> coordinatesFill = new List<PointD>();
            coordinatesFill.Add(new PointD(0, 0));
            coordinatesFill.Add(new PointD(10, 0));
            coordinatesFill.Add(new PointD(10, 10));
            coordinatesFill.Add(new PointD(0, 10));
            coordinatesFill.Add(new PointD(0, 0));

            var sectionFill = Substitute.For<ISection>();
            sectionFill.Area.Returns(100);
            sectionFill.CentreOfGravity.Returns(new PointD(5, 5));
            sectionFill.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            sectionFill.Coordinates.Returns(coordinatesFill);
            sectionFill.ModulusOfElasticity.Returns(200000000);
            sectionFill.ThermalCooeficient.Returns(0.00001);
            sectionFill.Type.Returns(SectionType.Fill);
            sectionFill.YMax.Returns(10);
            sectionFill.YMin.Returns(0);

            IList<PointD> coordinatesVoid = new List<PointD>();
            coordinatesVoid.Add(new PointD(2.5, 2.5));
            coordinatesVoid.Add(new PointD(7.5, 2.5));
            coordinatesVoid.Add(new PointD(7.5, 7.5));
            coordinatesVoid.Add(new PointD(2.5, 7.5));
            coordinatesVoid.Add(new PointD(2.5, 2.5));

            var sectionVoid = Substitute.For<ISection>();
            sectionVoid.Area.Returns(25);
            sectionVoid.CentreOfGravity.Returns(new PointD(5, 5));
            sectionVoid.MomentOfInertia.Returns(5 * 5 * 5 * 5 / 12);
            sectionVoid.Coordinates.Returns(coordinatesVoid);
            sectionVoid.ModulusOfElasticity.Returns(200000000);
            sectionVoid.ThermalCooeficient.Returns(0.00001);
            sectionVoid.Type.Returns(SectionType.Void);
            sectionVoid.YMax.Returns(7.5);
            sectionVoid.YMin.Returns(2.5);


            var compositeSection = Substitute.For<IIntegrable>();
            compositeSection.CentreOfGravity.Returns(new PointD(5, 5));
            //compositeSection.Height.Returns(10);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12 - 5 * 5 * 5 * 5 / 12);
            compositeSection.Sections.Returns(new List<ISection>() { sectionFill,sectionVoid });

            Integration interigation = new Integration(compositeSection, (e) => 1);
            interigation.Integrate();


            Assert.AreEqual(75, interigation.NormalForce, 0.00000001);
            Assert.AreEqual(0, interigation.Moment, 0.0000000001);
            
        }*/

        /*[Test()]
        public void IntegrateTest_CustomSectionFunctionConstant_Passed()
        {
            IList<PointD> coordinatesFill = new List<PointD>();
            coordinatesFill.Add(new PointD(0, 0));
            coordinatesFill.Add(new PointD(4,0 ));
            coordinatesFill.Add(new PointD(4,5 ));
            coordinatesFill.Add(new PointD(7,7 ));
            coordinatesFill.Add(new PointD(7,8 ));
            coordinatesFill.Add(new PointD(-3,8 ));
            coordinatesFill.Add(new PointD(-3,7 ));
            coordinatesFill.Add(new PointD(0, 5));
            coordinatesFill.Add(new PointD(0, 0));

            var sectionFill = Substitute.For<ISection>();
            //sectionFill.Area.Returns(100);
            sectionFill.CentreOfGravity.Returns(new PointD(2, 4.7954545454));
            //sectionFill.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            sectionFill.Coordinates.Returns(coordinatesFill);
            //sectionFill.ModulusOfElasticity.Returns(200000000);
            //sectionFill.ThermalCooeficient.Returns(0.00001);
            sectionFill.Type.Returns(SectionType.Fill);
            sectionFill.YMax.Returns(8);
            sectionFill.YMin.Returns(0);

            IList<PointD> coordinatesVoid = new List<PointD>();
            coordinatesVoid.Add(new PointD(2,2 ));
            coordinatesVoid.Add(new PointD(3,4 ));
            coordinatesVoid.Add(new PointD(2,6 ));
            coordinatesVoid.Add(new PointD(1,4 ));
            coordinatesVoid.Add(new PointD(2,2 ));

            var sectionVoid = Substitute.For<ISection>();
            sectionVoid.Area.Returns(25);
            sectionVoid.CentreOfGravity.Returns(new PointD(5, 5));
            //sectionVoid.MomentOfInertia.Returns(5 * 5 * 5 * 5 / 12);
            sectionVoid.Coordinates.Returns(coordinatesVoid);
            //sectionVoid.ModulusOfElasticity.Returns(200000000);
            //sectionVoid.ThermalCooeficient.Returns(0.00001);
            sectionVoid.Type.Returns(SectionType.Void);
            sectionVoid.YMax.Returns(6);
            sectionVoid.YMin.Returns(2);


            var compositeSection = Substitute.For<IIntegrable>();
            compositeSection.CentreOfGravity.Returns(new PointD(2, 4.875));
            //compositeSection.Height.Returns(8);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12 - 5 * 5 * 5 * 5 / 12);
            compositeSection.Sections.Returns(new List<ISection>() { sectionFill, sectionVoid });

            Integration interigation = new Integration(compositeSection, (e) => 1);
            interigation.Integrate();

            Assert.AreEqual(40, interigation.NormalForce,0.001);
            Assert.AreEqual(0, interigation.Moment, 0.001);
        }

        [Test()]
        public void IntegrateTest_CustomSectionFunctionLinear_Passed()
        {
            IList<PointD> coordinatesFill = new List<PointD>();
            coordinatesFill.Add(new PointD(0, 0));
            coordinatesFill.Add(new PointD(4, 0));
            coordinatesFill.Add(new PointD(4, 5));
            coordinatesFill.Add(new PointD(7, 7));
            coordinatesFill.Add(new PointD(7, 8));
            coordinatesFill.Add(new PointD(-3, 8));
            coordinatesFill.Add(new PointD(-3, 7));
            coordinatesFill.Add(new PointD(0, 5));
            coordinatesFill.Add(new PointD(0, 0));

            var sectionFill = Substitute.For<ISection>();
            //sectionFill.Area.Returns(100);
            sectionFill.CentreOfGravity.Returns(new PointD(2, 4.7954545454));
            //sectionFill.MomentOfInertia.Returns(10 * 10 * 10 * 10 / 12);
            sectionFill.Coordinates.Returns(coordinatesFill);
            //sectionFill.ModulusOfElasticity.Returns(200000000);
            //sectionFill.ThermalCooeficient.Returns(0.00001);
            sectionFill.Type.Returns(SectionType.Fill);
            sectionFill.YMax.Returns(8);
            sectionFill.YMin.Returns(0);

            IList<PointD> coordinatesVoid = new List<PointD>();
            coordinatesVoid.Add(new PointD(2, 2));
            coordinatesVoid.Add(new PointD(3, 4));
            coordinatesVoid.Add(new PointD(2, 6));
            coordinatesVoid.Add(new PointD(1, 4));
            coordinatesVoid.Add(new PointD(2, 2));

            var sectionVoid = Substitute.For<ISection>();
            sectionVoid.Area.Returns(25);
            sectionVoid.CentreOfGravity.Returns(new PointD(5, 5));
            //sectionVoid.MomentOfInertia.Returns(5 * 5 * 5 * 5 / 12);
            sectionVoid.Coordinates.Returns(coordinatesVoid);
            //sectionVoid.ModulusOfElasticity.Returns(200000000);
            //sectionVoid.ThermalCooeficient.Returns(0.00001);
            sectionVoid.Type.Returns(SectionType.Void);
            sectionVoid.YMax.Returns(6);
            sectionVoid.YMin.Returns(2);


            var compositeSection = Substitute.For<IIntegrable>();
            compositeSection.CentreOfGravity.Returns(new PointD(2, 4.875));
            //compositeSection.Height.Returns(8);
            //compositeSection.MomentOfIntertia.Returns(10 * 10 * 10 * 10 / 12 - 5 * 5 * 5 * 5 / 12);
            compositeSection.Sections.Returns(new List<ISection>() { sectionFill, sectionVoid });

            var integrationFunction = LinearSpline.InterpolateSorted(new double[] { 0, 8 }, new double[] { 0.301324503*1000, 0.627356088*1000 });
            Integration interigation = new Integration(compositeSection, integrationFunction.Interpolate);
            interigation.Integrate();

            Assert.AreEqual(20000, interigation.NormalForce, 0.001);
            Assert.AreEqual(-10000, interigation.Moment, 0.01);
        }*/
    }
}