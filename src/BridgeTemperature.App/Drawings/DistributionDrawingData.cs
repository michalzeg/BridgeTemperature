using System.Collections.Generic;
using BridgeTemperature.Calculations.Distributions;

namespace BridgeTemperature.Drawing
{
    public class DistributionDrawingData
    {
        public IList<Distribution> Distribution { get; set; }
        public double SectionMaxY { get; set; }
        public double SectionMinY { get; set; }
        public double SectionMaxX { get; set; }
        public double SectionMinX { get; set; }
    }
}