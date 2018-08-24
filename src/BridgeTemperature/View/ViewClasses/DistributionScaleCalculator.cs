﻿using System;

namespace BridgeTemperature.Drawing
{
    public class DistributionScaleCalculator : ScaleCalculator
    {
        public DistributionScaleCalculator(Func<double> actualWidth, Func<double> actualHeight)
            : base(actualWidth, actualHeight)
        {
        }

        protected override void calculateScale(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }
}