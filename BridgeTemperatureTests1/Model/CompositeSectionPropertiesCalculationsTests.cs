using NUnit.Framework;
using BridgeTemperature.SectionProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using BridgeTemperature.Extensions;
using BridgeTemperature.Sections;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.SectionProperties.Tests
{
    [TestFixture()]
    public class CompositeSectionPropertiesCalculationsTests
    {
        [Test()]
        public void CompositeSectionPropertiesCalculations_TwoRectanglesFromTheSameMaterial_Passed()
        {
            var section1 = Substitute.For<ICompositePropertiesCalculations>();
            section1.Area.Returns(24);
            section1.Type.Returns(SectionType.Fill);
            section1.ModulusOfElasticity.Returns(200000000);
            section1.CentreOfGravity.Returns(new PointD(0, 10));
            section1.MomentOfInertia.Returns(8);

            var section2 = Substitute.For<ICompositePropertiesCalculations>();
            section2.Area.Returns(24);
            section2.Type.Returns(SectionType.Fill);
            section2.ModulusOfElasticity.Returns(200000000);
            section2.CentreOfGravity.Returns(new PointD(0, 5));
            section2.MomentOfInertia.Returns(8);

            IList<ICompositePropertiesCalculations> listOfSections = new List<ICompositePropertiesCalculations>();
            listOfSections.Add(section1);
            listOfSections.Add(section2);

            var compositeProperties = new CompositeSectionPropertiesCalculations(listOfSections);

            Assert.AreEqual(7.5, compositeProperties.CentreOfGravity.Y);
            Assert.AreEqual(48, compositeProperties.Area);
            Assert.AreEqual(316, compositeProperties.SecondMomentOfArea);
        }

        [Test()]
        public void CompositeSectionPropertiesCalculations_TwoRectanglesFromDiferentMaterials_Passed()
        {
            var section1 = Substitute.For<ICompositePropertiesCalculations>();
            section1.Area.Returns(24);
            section1.Type.Returns(SectionType.Fill);
            section1.ModulusOfElasticity.Returns(10000000);
            section1.CentreOfGravity.Returns(new PointD(0, 10));
            section1.MomentOfInertia.Returns(8);

            var section2 = Substitute.For<ICompositePropertiesCalculations>();
            section2.Area.Returns(24);
            section2.Type.Returns(SectionType.Fill);
            section2.ModulusOfElasticity.Returns(210000000);
            section2.CentreOfGravity.Returns(new PointD(0, 5));
            section2.MomentOfInertia.Returns(8);

            IList<ICompositePropertiesCalculations> listOfSections = new List<ICompositePropertiesCalculations>();
            listOfSections.Add(section1);
            listOfSections.Add(section2);

            var compositeProperties = new CompositeSectionPropertiesCalculations(listOfSections);

            Assert.AreEqual(5.2272.Round(), compositeProperties.CentreOfGravity.Y.Round());
            Assert.AreEqual(25.1428.Round(), compositeProperties.Area.Round());
            Assert.AreEqual(35.65.Round(), compositeProperties.SecondMomentOfArea.Round());
        }

        [Test()]
        public void CompositeSectionPropertiesCalculations_RectangleWithVoid_Passed()
        {
            var section1 = Substitute.For<ICompositePropertiesCalculations>();
            section1.Area.Returns(100);
            section1.Type.Returns(SectionType.Fill);
            section1.ModulusOfElasticity.Returns(200000000);
            section1.CentreOfGravity.Returns(new PointD(5, 5));
            section1.MomentOfInertia.Returns(833.333333);

            var section2 = Substitute.For<ICompositePropertiesCalculations>();
            section2.Area.Returns(25);
            section2.Type.Returns(SectionType.Void);
            section2.ModulusOfElasticity.Returns(200000000);
            section2.CentreOfGravity.Returns(new PointD(5, 5));
            section2.MomentOfInertia.Returns(52.083);

            IList<ICompositePropertiesCalculations> listOfSections = new List<ICompositePropertiesCalculations>();
            listOfSections.Add(section1);
            listOfSections.Add(section2);

            var compositeProperties = new CompositeSectionPropertiesCalculations(listOfSections);

            Assert.AreEqual(5.0.Round(), compositeProperties.CentreOfGravity.Y.Round());
            Assert.AreEqual(75d.Round(), compositeProperties.Area.Round());
            Assert.AreEqual(781.25.Round(), compositeProperties.SecondMomentOfArea.Round());

            
        }
        
    }
}