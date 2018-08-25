using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Sections;
using System.Collections.Generic;

namespace BridgeTemperature.Drawing
{
    public class SectionDrawingData
    {
        public IList<PointD> Coordinates;
        public SectionType Type;
    }
}