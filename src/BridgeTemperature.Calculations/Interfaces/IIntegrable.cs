using System.Collections.Generic;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.IntegrationFunctions
{
    public interface IIntegrable
    {
        IList<PointD> Coordinates { get; }
        PointD CentreOfGravity { get; }
        SectionType Type { get; }
        double YMax { get; }
        double YMin { get; }
    }
}