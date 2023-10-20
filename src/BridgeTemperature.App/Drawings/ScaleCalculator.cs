using BridgeTemperature.Shared.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.Drawing
{
    public class ScaleCalculator
    {
        public double ScaleY { get; set; }
        public double ScaleX { get; set; }
        public PointD Centre { get; set; }
        public double MaxY { get; private set; }
        public double MinY { get; private set; }

        public Func<double> CanvasActualWidth;
        public Func<double> CanvasActualHeight;

        public ScaleCalculator(Func<double> actualWidth, Func<double> actualHeight)
        {
            CanvasActualHeight = actualHeight;
            CanvasActualWidth = actualWidth;
            Centre = new PointD(0, 0);
        }

        public void UpdateProperties(IList<IList<PointD>> perimeters)
        {
            if (perimeters == null || perimeters.Count == 0)
                return;

            var xMax = perimeters.Max(e => e.Max(g => g.X));
            var xMin = perimeters.Min(e => e.Min(g => g.X));
            var yMax = perimeters.Max(e => e.Max(g => g.Y));
            var yMin = perimeters.Min(e => e.Min(g => g.Y));

            MaxY = yMax;
            MinY = yMin;

            var drawingWidth = xMax - xMin;
            var drawingHeight = yMax - yMin;

            Centre = new PointD(drawingWidth / 2 + xMin, drawingHeight / 2 + yMin);
            var scaleX = CanvasActualWidth() / drawingWidth;
            var scaleY = CanvasActualHeight() / drawingHeight;

            CalculateScale(scaleX, scaleY);
        }

        protected virtual void CalculateScale(double scaleX, double scaleY)
        {
            var scale = Math.Min(scaleX, scaleY);
            ScaleY = scale;
            ScaleX = scale;
        }
    }
}