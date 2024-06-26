﻿using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Sections;
using System.Collections.Generic;

namespace BridgeTemperature.Calculations.Interfaces
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