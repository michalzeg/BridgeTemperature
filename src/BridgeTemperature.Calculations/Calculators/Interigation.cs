using System;
using System.Linq;
using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Calculations.Slicing;
using BridgeTemperature.Common.Geometry;
using BridgeTemperature.Common.Sections;

namespace BridgeTemperature.Calculations.Calculators
{
    public class Integration : IIntegration
    {
        private readonly int numberOfSlices = 1500;
        public double Moment { get; private set; }
        public double NormalForce { get; private set; }

        public Integration()
        {
        }

        public void Reset()
        {
            Moment = 0;
            NormalForce = 0;
        }

        public void Integrate(IIntegrable section, PointD integrationPoint, Func<double, double> distributionFunction)
        {
            double resultantMoment = 0;
            double resultantNormalForce = 0;
            var slicing = new SlicingCalculator();

            double sectionTypeMultiplier = (section.Type == SectionType.Void) ? -1 : 1;

            double currentY = section.YMin;
            double deltaY = (section.YMax - section.YMin) / this.numberOfSlices;
            do
            {
                var slice = slicing.GetSlice(section.Coordinates, currentY + deltaY, currentY);
                currentY = currentY + deltaY;
                double value = distributionFunction(slice.CentreOfGravityY);
                double normalForce = value * slice.Area * sectionTypeMultiplier;
                double leverArm = Math.Abs(integrationPoint.Y - slice.CentreOfGravityY);
                double moment = ((slice.CentreOfGravityY < integrationPoint.Y) ? leverArm * value * slice.Area : -leverArm * value * slice.Area) * sectionTypeMultiplier;

                resultantMoment = resultantMoment + moment;
                resultantNormalForce = resultantNormalForce + normalForce;
            }
            while (currentY <= section.YMax);
            this.NormalForce = this.NormalForce + resultantNormalForce;
            this.Moment = this.Moment + resultantMoment;
        }
    }
}