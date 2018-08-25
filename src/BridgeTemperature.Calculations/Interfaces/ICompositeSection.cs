using System.Collections.Generic;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.Sections
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