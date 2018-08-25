using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows;
using BridgeTemperature.Common.Geometry;

namespace BridgeTemperature.Drawing
{
    public class PolygonDrawing : DrawingBase
    {
        public PolygonDrawing(ScaleCalculator scaleProperties) : base(scaleProperties)
        {
        }

        public Polygon CreatePolygonDrawing(IList<PointD> section)
        {
            if (section == null || section.Count == 0)
                return null;

            var polygon = new Polygon();
            var transferedCoordinates = base.TransformCoordinatesToCentreOfGrid(section);
            foreach (var point in transferedCoordinates)
            {
                polygon.Points.Add(new Point(point.X, point.Y));
            }
            return polygon;
        }
    }
}