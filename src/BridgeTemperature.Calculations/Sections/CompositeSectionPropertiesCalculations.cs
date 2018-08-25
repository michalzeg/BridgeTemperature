using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Common.Geometry;
using BridgeTemperature.Common.Sections;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.Calculations.Sections
{
    public class CompositeSectionPropertiesCalculations
    {
        public double BaseModulusOfElasticity { get; private set; }
        public PointD CentreOfGravity { get; private set; }
        public double Area { get; private set; }
        public double SecondMomentOfArea { get; private set; }
        private IEnumerable<ICompositePropertiesCalculations> sections;

        public CompositeSectionPropertiesCalculations(IEnumerable<ICompositePropertiesCalculations> sections)
        {
            this.sections = sections;
            this.getBaseModulusOfElasticity();
            this.calculateSectionProperties();
        }

        private void getBaseModulusOfElasticity()
        {
            this.BaseModulusOfElasticity = this.sections.Max(e => e.ModulusOfElasticity);
        }

        private void calculateSectionProperties()
        {
            double firstMomentOfAreaX = 0;
            double firstMomentOfAreaY = 0;
            double secondMomentOfAreaGlobalX = 0;
            double area = 0;
            foreach (var section in this.sections)
            {
                double multiplier = section.Type == SectionType.Void ? -1 : 1;

                double alfa = this.BaseModulusOfElasticity / section.ModulusOfElasticity;
                area = area + multiplier * section.Area / alfa;
                firstMomentOfAreaX = firstMomentOfAreaX + multiplier * section.Area / alfa * section.CentreOfGravity.Y;
                firstMomentOfAreaY = firstMomentOfAreaY + multiplier * section.Area / alfa * section.CentreOfGravity.X;
                secondMomentOfAreaGlobalX = secondMomentOfAreaGlobalX + multiplier * (section.MomentOfInertia / alfa + section.Area / alfa * section.CentreOfGravity.Y * section.CentreOfGravity.Y);
            }
            this.Area = area;
            double y0 = firstMomentOfAreaX / area;
            double x0 = firstMomentOfAreaY / area;

            this.CentreOfGravity = new PointD(x0, y0);
            this.SecondMomentOfArea = secondMomentOfAreaGlobalX - area * y0 * y0;
        }
    }
}