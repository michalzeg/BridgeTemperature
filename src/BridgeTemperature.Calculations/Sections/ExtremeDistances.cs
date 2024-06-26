﻿using BridgeTemperature.Shared.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.Calculations.Sections
{
    public class ExtremeDistances
    {
        private readonly PointD _centreOfGravity;

        public ExtremeDistances(PointD centreOfGravity)
        {
            this._centreOfGravity = centreOfGravity;
        }

        public void MaxDistancesCentralCoordinateSystem(IList<PointD> coordinates, out double x0_max, out double x0_min, out double y0_max, out double y0_min)
        {
            x0_max = coordinates.Max(point => point.X) - _centreOfGravity.X;
            x0_min = coordinates.Min(point => point.X) - _centreOfGravity.X;
            y0_max = coordinates.Max(point => point.Y) - _centreOfGravity.Y;
            y0_min = coordinates.Min(point => point.Y) - _centreOfGravity.Y;
        }

        public void MaxDistancesPrincipalCoordinateSystem(IList<PointD> coordinates, double alfa, out double x_max, out double x_min, out double y_max, out double y_min)
        {
            double cos = Math.Cos(alfa);
            double sin = Math.Sin(alfa);
            double xo = _centreOfGravity.X * cos - _centreOfGravity.Y * sin;
            double yo = _centreOfGravity.X * sin + _centreOfGravity.Y * cos;

            x_max = coordinates.Max(point => point.X * cos - point.Y * sin) - xo;
            x_min = coordinates.Min(point => point.X * cos - point.Y * sin) - xo;
            y_max = coordinates.Max(point => point.X * sin + point.Y * cos) - yo;
            y_min = coordinates.Min(point => point.X * sin + point.Y * cos) - yo;
        }

        public void ExtremeCoordinates(IList<PointD> coordinates, out double xmax, out double xmin, out double ymax, out double ymin)
        {
            xmax = coordinates.Max(point => point.X);
            ymax = coordinates.Max(point => point.Y);
            xmin = coordinates.Min(point => point.X);
            ymin = coordinates.Min(point => point.Y);
        }
    }
}