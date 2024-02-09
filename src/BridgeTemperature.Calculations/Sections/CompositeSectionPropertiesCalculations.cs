using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Sections;
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
        private readonly IEnumerable<ICompositePropertiesCalculations> _sections;

        public CompositeSectionPropertiesCalculations(IEnumerable<ICompositePropertiesCalculations> sections)
        {
            _sections = sections;
            GetBaseModulusOfElasticity();
            CalculateSectionProperties();
        }

        private void GetBaseModulusOfElasticity()
        {
            BaseModulusOfElasticity = _sections.Max(e => e.ModulusOfElasticity);
        }

        private void CalculateSectionProperties()
        {
            double firstMomentOfAreaX = 0;
            double firstMomentOfAreaY = 0;
            double secondMomentOfAreaGlobalX = 0;
            double area = 0;
            foreach (var section in _sections)
            {
                double multiplier = section.Type == SectionType.Void ? -1 : 1;

                double alfa = BaseModulusOfElasticity / section.ModulusOfElasticity;
                area += multiplier * section.Area / alfa;
                firstMomentOfAreaX += multiplier * section.Area / alfa * section.CentreOfGravity.Y;
                firstMomentOfAreaY += multiplier * section.Area / alfa * section.CentreOfGravity.X;
                secondMomentOfAreaGlobalX += multiplier * (section.MomentOfInertia / alfa + section.Area / alfa * section.CentreOfGravity.Y * section.CentreOfGravity.Y);
            }
            Area = area;
            double y0 = firstMomentOfAreaX / area;
            double x0 = firstMomentOfAreaY / area;

            CentreOfGravity = new PointD(x0, y0);
            SecondMomentOfArea = secondMomentOfAreaGlobalX - area * y0 * y0;
        }
    }
}