using NUnit.Framework;
using BridgeTemperature.DistributionOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace BridgeTemperature.DistributionOperations.Tests
{
    [TestFixture]
    public class DistributionOperationsTests
    {
        [Test]
        public void DistributionOperationGetValue_ConstantDistribution_Passed()
        {
            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 5 },
                new Distribution() { Y = 9.9999999, Value = 5 },
                new Distribution() { Y = 10, Value = 10 },
                new Distribution() { Y = 20, Value = 10 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution);

            var actualInterPolatedValue = distributionOperations.GetValue(10);

            Assert.AreEqual(10, actualInterPolatedValue);
        }

        [Test]
        public void AddDistribution_AddTwoConstantDistributions_Passed()
        {
            var distribution1 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 2, Value = 10 },
                new Distribution() { Y = 10, Value = 10 }
            };
            var distribution2 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 0.5 },
                new Distribution() { Y = 5, Value = 0.5 },
                new Distribution() { Y = 10, Value = 0.5 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution1);
            distributionOperations.AddDistribution(distribution2);

            var expectedDistribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10.5 },
                new Distribution() { Y = 2, Value = 10.5 },
                new Distribution() { Y = 5, Value = 10.5 },
                new Distribution() { Y = 10, Value = 10.5 }
            };

            Assert.AreEqual(expectedDistribution, distributionOperations.Distribution);
        }

        [Test]
        public void SubtrackDistribution_SubtractConstantDistribution_Passed()
        {
            var distribution1 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 7, Value = 10 },
                new Distribution() { Y = 10, Value = 10 }
            };
            var distribution2 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 0.5 },
                new Distribution() { Y = 5, Value = 0.5 },
                new Distribution() { Y = 8, Value = 0.5 },
                new Distribution() { Y = 10, Value = 0.5 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution1);
            distributionOperations.SubtractDistribution(distribution2);

            var expectedDistribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 9.5 },
                new Distribution() { Y = 5, Value = 9.5 },
                new Distribution() { Y = 7, Value = 9.5 },
                new Distribution() { Y = 8, Value = 9.5 },
                new Distribution() { Y = 10, Value = 9.5 }
            };

            Assert.AreEqual(expectedDistribution, distributionOperations.Distribution);
        }

        [Test]
        public void AddDistribution_AddTwoLinearDistributions_Passed()
        {
            var distribution1 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 0 },
                new Distribution() { Y = 2, Value = 2 },
                new Distribution() { Y = 10, Value = 10 }
            };
            var distribution2 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 20 },
                new Distribution() { Y = 5, Value = 20 },
                new Distribution() { Y = 10, Value = 20 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution1);
            distributionOperations.AddDistribution(distribution2);

            var expectedDistribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 20 },
                new Distribution() { Y = 2, Value = 22 },
                new Distribution() { Y = 5, Value = 25 },
                new Distribution() { Y = 10, Value = 30 }
            };

            Assert.AreEqual(expectedDistribution, distributionOperations.Distribution);
        }

        [Test]
        public void SubtractDistribution_SubTractTwoLinearDistributions_Passed()
        {
            var distribution1 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 7, Value = 10 },
                new Distribution() { Y = 10, Value = 10 }
            };
            var distribution2 = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 0 },
                new Distribution() { Y = 5, Value = 10 },
                new Distribution() { Y = 8, Value = 16 },
                new Distribution() { Y = 10, Value = 20 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution1);
            distributionOperations.SubtractDistribution(distribution2);

            var expectedDistribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 5, Value = 0 },
                new Distribution() { Y = 7, Value = -4 },
                new Distribution() { Y = 8, Value = -6 },
                new Distribution() { Y = 10, Value = -10 }
            };

            Assert.AreEqual(expectedDistribution, distributionOperations.Distribution);
        }

        [Test()]
        public void MultiplyDistributionTest()
        {
            var distribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = 10 },
                new Distribution() { Y = 10, Value = 2 }
            };

            var distributionOperations = Substitute.ForPartsOf<BaseDistribution>(distribution);
            distributionOperations.MultiplyDistribution(-1.5);

            var expectedDistribution = new List<Distribution>
            {
                new Distribution() { Y = 0, Value = -15 },
                new Distribution() { Y = 10, Value = -3 }
            };

            Assert.AreEqual(expectedDistribution, distributionOperations.Distribution);
        }
    }
}