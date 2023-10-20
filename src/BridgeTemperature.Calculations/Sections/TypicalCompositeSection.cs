using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Shared.Geometry;
using System.Collections.Generic;

namespace BridgeTemperature.Calculations.Sections
{
    public class TypicalCompositeSection : ICompositeSection
    {
        public double BaseModulusOfElasticity { get; private set; }
        public PointD CentreOfGravity { get; private set; }
        public double MomentOfIntertia { get; private set; }
        public ICollection<ISection> Sections { get; private set; }
        public double Area { get; private set; }

        public TypicalCompositeSection(ICollection<ISection> sections)
        {
            var compositeProperties = new CompositeSectionPropertiesCalculations(sections);
            Sections = sections;
            BaseModulusOfElasticity = compositeProperties.BaseModulusOfElasticity;
            CentreOfGravity = compositeProperties.CentreOfGravity;
            Area = compositeProperties.Area;
            MomentOfIntertia = compositeProperties.SecondMomentOfArea;
        }
    }
}