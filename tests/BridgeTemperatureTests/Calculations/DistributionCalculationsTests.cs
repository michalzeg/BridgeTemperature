using NUnit.Framework;
using BridgeTemperature.DistributionOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Sections;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.DistributionOperations.Tests
{
    [TestFixture]
    public class DistributionCalculationsTests
    {
        [Test]
        public void CalculateDistributions_ClarkExample_Passed()
        {
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(1, 0));
            coordinates.Add(new PointD(1, 1));
            coordinates.Add(new PointD(0, 1));

            var thermalCoefficient = 12d / 1000000d;
            var modulusOfElasticity = 28000000d;

            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 2.5 },
                new Distribution() { Y = 0.2, Value = 0 },
                new Distribution() { Y = 0.6, Value = 0 },
                new Distribution() { Y = 0.85, Value = 2.4 },
                new Distribution() { Y = 1, Value = 10.8 }
            };
            var section = new Section(coordinates, SectionType.Steel, modulusOfElasticity, thermalCoefficient, distribution);
            var compositeSection = new TypicalCompositeSection(new List<ISection>() { section });

            var calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();
            var expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            {
                new Distribution { Y=0,Value=-1145},
                new Distribution {Y=0.2,Value=23 },
                new Distribution {Y=0.6,Value=682 },
                new Distribution {Y=0.85,Value=287 },
                new Distribution {Y=1,Value=-2288 }
            });

            var actualSelfEquilibratedStress = calculations.GetResult(ResultType.SelfEquilibratedStress);

            for (int i = 0; i <= expectedSelfEquilibratedStress.Count - 1; i++)
            {
                List<Distribution> actual = actualSelfEquilibratedStress.ToList()[i].ToList();
                List<Distribution> expected = expectedSelfEquilibratedStress[i];

                for (int j = 0; j <= actual.Count - 1; j++)
                {
                    Assert.AreEqual(expected[j].Value, actual[j].Value, 1);
                    Assert.AreEqual(expected[j].Y, actual[j].Y, 1);
                }
            }
        }

        [Test]
        public void CalculateDistributions_OwnExample_Passed()
        {
            List<PointD> coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(1, 0),
                new PointD(1, 3),
                new PointD(0, 3)
            };

            var thermalCoefficient = 12d / 1000000d;
            var modulusOfElasticity = 205000000d;

            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 1.5, Value = 0 },
                new Distribution() { Y = 3, Value = 10 }
            };

            var section = new Section(coordinates, SectionType.Steel, modulusOfElasticity, thermalCoefficient, distribution);
            var compositeSection = new TypicalCompositeSection(new List<ISection>() { section });

            var calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();

            var expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            {
                new Distribution { Y=0,Value=-6150},
                new Distribution {Y=1.5,Value=6150 },
                new Distribution {Y=3,Value=-6150 }
            });

            var actualSelfEquilibratedStress = calculations.GetResult(ResultType.SelfEquilibratedStress);

            for (int i = 0; i <= expectedSelfEquilibratedStress.Count - 1; i++)
            {
                List<Distribution> actual = actualSelfEquilibratedStress.ToList()[i].ToList();
                List<Distribution> expected = expectedSelfEquilibratedStress[i];

                for (int j = 0; j <= actual.Count - 1; j++)
                {
                    Assert.AreEqual(expected[j].Value, actual[j].Value, 0.1);
                    Assert.AreEqual(expected[j].Y, actual[j].Y, 0.1);
                }
            }
        }

        [Test]
        public void CalculateDistributions_OwnExampleTwoSections_Passed()
        {
            var thermalCoefficient = 12d / 1000000d;
            var modulusOfElasticity = 205000000d;

            var coordinates1 = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(1, 0),
                new PointD(1, 1.5),
                new PointD(0, 1.5)
            };

            var distribution1 = new List<Distribution>();
            var section1 = new Section(coordinates1, SectionType.Steel, modulusOfElasticity, thermalCoefficient, distribution1);

            var coordinates2 = new List<PointD>
            {
                new PointD(0, 1.5),
                new PointD(1, 1.5),
                new PointD(1, 3),
                new PointD(0, 3)
            };

            var distribution2 = new List<Distribution>
            {
                new Distribution() { Y = 1.5, Value = 10 },
                new Distribution() { Y = 3, Value = 10 }
            };

            var section2 = new Section(coordinates2, SectionType.Steel, modulusOfElasticity, thermalCoefficient, distribution2);
            var compositeSection = new TypicalCompositeSection(new List<ISection>() { section1, section2 });

            var calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();

            var expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            {
                new Distribution { Y=0,Value=-6150},
                new Distribution {Y=1.5,Value=12300 },
            });
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            {
                new Distribution { Y=1.5,Value=-12300},
                new Distribution {Y=3,Value=6150 },
             });

            var actualSelfEquilibratedStress = calculations.GetResult(ResultType.SelfEquilibratedStress);

            for (int i = 0; i <= expectedSelfEquilibratedStress.Count - 1; i++)
            {
                List<Distribution> actual = actualSelfEquilibratedStress.ToList()[i].ToList();
                List<Distribution> expected = expectedSelfEquilibratedStress[i];

                for (int j = 0; j <= actual.Count - 1; j++)
                {
                    Assert.AreEqual(expected[j].Value, actual[j].Value, 0.1);
                    Assert.AreEqual(expected[j].Y, actual[j].Y, 0.1);
                }
            }
        }
    }
}