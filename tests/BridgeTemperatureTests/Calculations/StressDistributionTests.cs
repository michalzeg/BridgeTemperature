﻿using NUnit.Framework;
using BridgeTemperature.DistributionOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.DistributionOperations.Tests
{
    [TestFixture()]
    public class StressDistributionTests
    {
        [Test()]
        public void ConvertToTemperatureDistribution_RectangleSectionPassed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 10));
            coordinates.Add(new PointD(0, 10));
            coordinates.Add(new PointD(0, 0));

            var distribution = new List<Distribution>();
            distribution.Add(new Distribution() { Y = 0, Value = 10 });
            distribution.Add(new Distribution() { Y = 5, Value = 5 });

            StressDistribution stressDistribution = new StressDistribution(distribution);

            var actualTemperatureDistribution = stressDistribution.ConvertToTemperatureDistribution(coordinates, 200000000, 0.000012);

            var expectedDistribution = new List<Distribution>();
            expectedDistribution.Add(new Distribution() { Y = 0, Value = 0.0041666667 });
            expectedDistribution.Add(new Distribution() { Y = 5, Value = 0.0020833333 });
            expectedDistribution.Add(new Distribution() { Y = 10, Value = 0 });

            Assert.AreEqual(expectedDistribution, actualTemperatureDistribution.Distribution);
        }

        [Test()]
        public void GetBendingStresses_RectangleSectionPositiveMoment_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 12));
            coordinates.Add(new PointD(0, 12));
            coordinates.Add(new PointD(0, 0));

            var bendingMoment = 1000000;
            var momentOfInertia = 1440d;
            var baseModulusOfElasticity = 200000000d;
            var modulusOfElasticity = 20000d;
            var actualStressDistribution = StressDistribution.BendingStress(coordinates, bendingMoment, 6, momentOfInertia, baseModulusOfElasticity, modulusOfElasticity);

            var expectedStressDistribution = new List<Distribution>();
            expectedStressDistribution.Add(new Distribution() { Y = 0, Value = 0.4166666666666667 });
            expectedStressDistribution.Add(new Distribution() { Y = 12, Value = -0.4166666666666667 });
            Assert.AreEqual(expectedStressDistribution, actualStressDistribution.Distribution);
        }

        [Test()]
        public void GetBendingStresses_RectangleSectionNegativeMoment_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 12));
            coordinates.Add(new PointD(0, 12));
            coordinates.Add(new PointD(0, 0));

            var bendingMoment = -2000000;
            var momentOfInertia = 1440d;
            var baseModulusOfElasticity = 200000000d;
            var modulusOfElasticity = 20000d;

            var actualStressDistribution = StressDistribution.BendingStress(coordinates, bendingMoment, 6, momentOfInertia, baseModulusOfElasticity, modulusOfElasticity);

            var expectedStressDistribution = new List<Distribution>();
            expectedStressDistribution.Add(new Distribution() { Y = 0, Value = -0.8333333333333334 });
            expectedStressDistribution.Add(new Distribution() { Y = 12, Value = 0.8333333333333334 });
            Assert.AreEqual(expectedStressDistribution, actualStressDistribution.Distribution);
        }

        [Test()]
        public void GetAxialDistribution_RectangleSection_Passed()
        {
            IList<PointD> coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(10, 0));
            coordinates.Add(new PointD(10, 12));
            coordinates.Add(new PointD(0, 12));
            coordinates.Add(new PointD(0, 0));

            var axialForce = 10000000d;
            var baseModulusOfElasticity = 200000000d;
            var modulusOfElasticity = 20000d;
            var area = 120d;

            var actualStressDistribution = StressDistribution.AxialStress(coordinates, axialForce, area, baseModulusOfElasticity, modulusOfElasticity);

            var expectedStressDistribution = new List<Distribution>();
            expectedStressDistribution.Add(new Distribution() { Y = 0, Value = 8.333333333333333333333333333 });
            expectedStressDistribution.Add(new Distribution() { Y = 12, Value = 8.333333333333333333333333333 });
            Assert.AreEqual(expectedStressDistribution, actualStressDistribution.Distribution);
        }
    }
}