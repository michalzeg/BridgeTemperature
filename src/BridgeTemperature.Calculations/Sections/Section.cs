using System;
using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Calculations.Distributions;
using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Shared.Sections;
using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Extensions;

namespace BridgeTemperature.Calculations.Sections
{
    public class Section : ISection
    {
        public StressDistribution ExternalStress { get; private set; }
        public StressDistribution BendingStress { get; set; }
        public StressDistribution UniformStress { get; set; }
        public StressDistribution SelfEquilibratedStress { get; set; }

        public TemperatureDistribution ExternalTemperature { get; private set; }
        public TemperatureDistribution BendingTemperature { get; set; }
        public TemperatureDistribution UniformTemperature { get; set; }
        public TemperatureDistribution SelfEquilibratedTemperature { get; set; }

        public SectionType Type { get; }
        public double ModulusOfElasticity { get; }
        public double ThermalCooeficient { get; }
        public double MomentOfInertia { get; }
        public double Area { get; }
        public PointD CentreOfGravity { get; }
        public double YMax { get; }
        public double YMin { get; }
        public double XMax { get; }
        public double XMin { get; }
        public double Height { get; }
        public IList<PointD> Coordinates { get; }

        public Section(IList<PointD> coordinates, SectionType type, double modulusOfElasticity, double thermalCooefficient, IEnumerable<Distribution> externalTemperatureDistribution)
        {
            this.Coordinates = CheckIfCoordinatesAreClockwise(coordinates);
            this.CheckLastElement();
            this.Type = type;
            this.ModulusOfElasticity = modulusOfElasticity;
            this.ThermalCooeficient = thermalCooefficient;

            var properties = new SectionPropertiesCalculations(this.Coordinates);

            this.Area = properties.A;
            this.CentreOfGravity = properties.CentreOfGravity;
            this.MomentOfInertia = properties.Ix0;
            this.YMax = properties.YMax;
            this.YMin = properties.YMin;
            this.XMax = properties.XMax;
            this.XMin = properties.XMin;
            this.Height = properties.YMax - properties.YMin;

            this.ExternalTemperature = new TemperatureDistribution(externalTemperatureDistribution.OrderBy(e => e.Y));
            this.ExternalStress = this.ExternalTemperature.ConvertToStressDistribution(coordinates, modulusOfElasticity, thermalCooefficient);
        }

        private void CheckLastElement()
        {
            PointD firstPoint = this.Coordinates[0];
            PointD lastPoint = this.Coordinates.Last();

            if (firstPoint.X.IsApproximatelyEqualTo(lastPoint.X) && firstPoint.Y.IsApproximatelyEqualTo(lastPoint.Y))
            {
                return;
            }
            else
            {
                this.Coordinates.Add(firstPoint);
            }
        }

        private IList<PointD> CheckIfCoordinatesAreClockwise(IList<PointD> coordinates)
        {
            if (coordinates.Count < 3)
                throw new ArgumentOutOfRangeException();

            var tempCoord = new List<PointD>(coordinates);
            for (int i = 0; i <= coordinates.Count - 3; i++)
            {
                var crossPrd = this.CrossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                if (crossPrd > 0)
                {
                    //clockwise
                    break;
                }
                else if (crossPrd < 0)
                {
                    //counterclockwise
                    tempCoord.Reverse();
                    break;
                }
            }

            return tempCoord;
        }

        private double CrossProduct(PointD point0, PointD point1, PointD point2)
        {
            double[] vector1 = new double[2];
            double[] vector2 = new double[2];

            vector1[0] = point1.X - point0.X;
            vector1[1] = point1.Y - point0.Y;
            vector2[0] = point2.X - point1.X;
            vector2[1] = point2.Y - point1.Y;

            double result;
            result = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return result;
        }
    }
}