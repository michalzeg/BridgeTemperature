using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BridgeTemperature.Helpers;

namespace BridgeTemperature.Drawing
{
    public abstract class DrawingBase
    {
        protected ScaleCalculator scaleProperties;

        public DrawingBase(ScaleCalculator scaleProperties)
        {
            this.scaleProperties = scaleProperties;
        }

        protected IList<PointD> transformCoordinatesToCentreOfGrid(IList<PointD> coordinates)
        {
            var pointList = new List<PointD>();
            foreach (var point in coordinates)
            {
                var newPoint = transformCoordinatesToCentreOfGrid(point);
                pointList.Add(newPoint);
            }
            return pointList;
        }

        protected PointD transformCoordinatesToCentreOfGrid(PointD point)
        {
            var newPoint = new PointD();
            newPoint.X = ((point.X - this.scaleProperties.Centre.X) * scaleProperties.ScaleX) + scaleProperties.CanvasActualWidth() / 2;
            newPoint.Y = (-(point.Y - this.scaleProperties.Centre.Y) * scaleProperties.ScaleY) + scaleProperties.CanvasActualHeight() / 2;
            return newPoint;
        }

        protected PointD transformCoordinatesToCentreOfGrid(double x, double y)
        {
            return transformCoordinatesToCentreOfGrid(new PointD(x, y));
        }

        public PointD TransformCoordinatesFromCentreOfGrid(PointD point)
        {
            var newPoint = new PointD();
            newPoint.X = ((point.X - scaleProperties.CanvasActualWidth() / 2) / scaleProperties.ScaleX) + scaleProperties.Centre.X;
            newPoint.Y = (-1) * ((point.Y - scaleProperties.CanvasActualHeight() / 2) / scaleProperties.ScaleY) + scaleProperties.Centre.Y;
            return newPoint;
        }

        public PointD TransformCoordinatesFromCentreOfGrid(Point point)
        {
            return TransformCoordinatesFromCentreOfGrid(new PointD(point.X, point.Y));
        }
    }
}