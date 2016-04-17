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
    [TestFixture()]
    public class DistributionCalculationsTests
    {
        [Test()]
        public void CalculateDistributions_ClarkExample_Passed()
        {
            List<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(1, 0));
            coordinates.Add(new PointD(1, 1));
            coordinates.Add(new PointD(0, 1));

            double thermalCoefficient = 12d / 1000000d;
            double modulusOfElasticity = 28000000d;

            List<Distribution> distribution = new List<Distribution>();
            distribution.Add(new Distribution() { Y = 0, Value = 2.5 });
            distribution.Add(new Distribution() { Y = 0.2, Value = 0 });
            distribution.Add(new Distribution() { Y = 0.6, Value = 0 });
            distribution.Add(new Distribution() { Y = 0.85, Value = 2.4 });
            distribution.Add(new Distribution() { Y = 1, Value = 10.8 });


            ISection section = new Section(coordinates, SectionType.Fill, modulusOfElasticity, thermalCoefficient, distribution);
            ICompositeSection compositeSection = new CompositeSection(new List<ISection>() { section });

            DistributionCalculations calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();


            List<List<Distribution>> expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            { new Distribution { Y=0,Value=-1300},
            new Distribution {Y=0.2,Value=0 },
            new Distribution {Y=0.6,Value=700 },
            new Distribution {Y=0.85,Value=300 },
            new Distribution {Y=1,Value=-2300 }
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

                //Assert.AreEqual(expected,actual);

            }
        }

        [Test()]
        public void CalculateDistributions_BoxSection_Passed()
        {
            
            Assert.Fail();
        }

        [Test()]
        public void CalculateDistributions_OwnExample_Passed()
        {
            List<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(1, 0));
            coordinates.Add(new PointD(1, 3));
            coordinates.Add(new PointD(0, 3));

            double thermalCoefficient = 12d / 1000000d;
            double modulusOfElasticity = 205000000d;

            List<Distribution> distribution = new List<Distribution>();
            distribution.Add(new Distribution() { Y = 1.5, Value = 0 });
            distribution.Add(new Distribution() { Y = 3, Value = 10 });



            ISection section = new Section(coordinates, SectionType.Fill, modulusOfElasticity, thermalCoefficient, distribution);
            ICompositeSection compositeSection = new CompositeSection(new List<ISection>() { section });

            DistributionCalculations calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();


            List<List<Distribution>> expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            { new Distribution { Y=0,Value=-6150},
            new Distribution {Y=1.5,Value=6150 },
            new Distribution {Y=3,Value=-6150 } });

            var actualSelfEquilibratedStress = calculations.GetResult(ResultType.SelfEquilibratedStress);

            for (int i =0;i<=expectedSelfEquilibratedStress.Count-1;i++)
            {
                List<Distribution> actual = actualSelfEquilibratedStress.ToList()[i].ToList();
                List<Distribution> expected = expectedSelfEquilibratedStress[i];

                for (int j =0;j<=actual.Count-1;j++)
                {
                    Assert.AreEqual(expected[j].Value, actual[j].Value, 0.1);
                    Assert.AreEqual(expected[j].Y, actual[j].Y, 0.1);
                }

                 //Assert.AreEqual(expected,actual);
            }

            
        }
        [Test()]
        public void CalculateDistributions_OwnExampleTwoSections_Passed()
        {
            
            double thermalCoefficient = 12d / 1000000d;
            double modulusOfElasticity = 205000000d;

            List<PointD> coordinates1 = new List<PointD>();
            coordinates1.Add(new PointD(0, 0));
            coordinates1.Add(new PointD(1, 0));
            coordinates1.Add(new PointD(1, 1.5));
            coordinates1.Add(new PointD(0, 1.5));

            List<Distribution> distribution1 = new List<Distribution>();
            //distribution1.Add(new Distribution() { Y = 0, Value = 0 });
            //distribution1.Add(new Distribution() { Y = 0, Value = 0 });
            ISection section1 = new Section(coordinates1, SectionType.Fill, modulusOfElasticity, thermalCoefficient, distribution1);

            List<PointD> coordinates2 = new List<PointD>();
            coordinates2.Add(new PointD(0, 1.5));
            coordinates2.Add(new PointD(1, 1.5));
            coordinates2.Add(new PointD(1, 3));
            coordinates2.Add(new PointD(0, 3));

            List<Distribution> distribution2 = new List<Distribution>();
            distribution2.Add(new Distribution() { Y = 1.5, Value = 10 });
            distribution2.Add(new Distribution() { Y = 3, Value = 10 });
            ISection section2 = new Section(coordinates2, SectionType.Fill, modulusOfElasticity, thermalCoefficient, distribution2);


            ICompositeSection compositeSection = new CompositeSection(new List<ISection>() { section1, section2 });

            DistributionCalculations calculations = new DistributionCalculations(compositeSection);
            calculations.CalculateDistributions();


            List<List<Distribution>> expectedSelfEquilibratedStress = new List<List<Distribution>>();
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            { new Distribution { Y=0,Value=-6150},
            new Distribution {Y=1.5,Value=12300 },
            });
            expectedSelfEquilibratedStress.Add(new List<Distribution>()
            { new Distribution { Y=1.5,Value=-12300},
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

                //Assert.AreEqual(expected,actual);
            }


        }
    }
}