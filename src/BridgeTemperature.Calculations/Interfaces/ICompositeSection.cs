using System.Collections.Generic;
using BridgeTemperature.Shared.Geometry;

namespace BridgeTemperature.Calculations.Interfaces
{
    public interface ICompositeSection
    {
        ICollection<ISection> Sections { get; }
        PointD CentreOfGravity { get; }
        double MomentOfIntertia { get; }
        double BaseModulusOfElasticity { get; }
        double Area { get; }
    }
}