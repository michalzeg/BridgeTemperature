using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using BridgeTemperature.Common.Geometry;
using BridgeTemperature.Calculations.Distributions;

namespace BridgeTemperature.Drawing
{
    public class DistributionDrawing : Grid
    {
        static DistributionDrawing()
        {
            var metaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnDistributionChanged));
            metaData.AffectsRender = true;
            metaData.BindsTwoWayByDefault = true;
            DistributionDependencyProperty = DependencyProperty.Register("DistributionData", typeof(List<DistributionDrawingData>), typeof(DistributionDrawing), metaData);
            SectionCanvasHeightDependencyProperty = DependencyProperty.Register("SectionCanvasHeight", typeof(double), typeof(DistributionDrawing));
            SectionCanvasWidthDependencyProperty = DependencyProperty.Register("SectionCanvasWidth", typeof(double), typeof(DistributionDrawing));
        }

        public static readonly DependencyProperty DistributionDependencyProperty;

        public IList<DistributionDrawingData> DistributionData
        {
            get { return GetValue(DistributionDependencyProperty) as IList<DistributionDrawingData>; }
            set { SetValue(DistributionDependencyProperty, value); }
        }

        public static readonly DependencyProperty SectionCanvasHeightDependencyProperty;

        public double SectionCanvasHeight
        {
            get { return (double)GetValue(SectionCanvasHeightDependencyProperty); }
            set { SetValue(SectionCanvasHeightDependencyProperty, value); }
        }

        public static readonly DependencyProperty SectionCanvasWidthDependencyProperty;

        public double SectionCanvasWidth
        {
            get { return (double)GetValue(SectionCanvasWidthDependencyProperty); }
            set { SetValue(SectionCanvasWidthDependencyProperty, value); }
        }

        public static void OnDistributionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as DistributionDrawing;
            canvas.DistributionData = e.NewValue as List<DistributionDrawingData>;
            canvas.RefreshDrawing();
        }

        private ScaleCalculator distributionScaleCalculator;
        private ScaleCalculator sectionScaleCalculator;

        public DistributionDrawing()
        {
            distributionScaleCalculator = new DistributionScaleCalculator(() => ActualWidth, () => ActualHeight);
            sectionScaleCalculator = new ScaleCalculator(() => SectionCanvasWidth, () => SectionCanvasHeight);
        }

        public void RefreshDrawing()
        {
            Children.Clear();
            if (DistributionData == null || DistributionData.Count == 0)
            {
                return;
            }
            var sectionCoordinates = new List<IList<PointD>>
            {
                SectionBoxCoordinates()
            };
            sectionScaleCalculator.UpdateProperties(sectionCoordinates);

            var distributionCoordinates = new List<IList<PointD>>();
            foreach (var distribution in DistributionData)
            {
                if (distribution.Distribution == null || distribution.Distribution.Count == 0)
                    continue;
                var distributionPoints = this.GetDistributionCoordinates(distribution.Distribution);
                distributionCoordinates.Add(distributionPoints);
            }
            distributionScaleCalculator.UpdateProperties(distributionCoordinates);
            distributionScaleCalculator.ScaleY = sectionScaleCalculator.ScaleY;
            distributionScaleCalculator.Centre.Y = sectionScaleCalculator.Centre.Y;

            foreach (var distribution in DistributionData)
            {
                if (distribution.Distribution == null || distribution.Distribution.Count == 0)
                    continue;
                PolygonDrawing drawing = new PolygonDrawing(distributionScaleCalculator);
                var popup = new DistributionPopup();
                var distributionPoints = this.GetDistributionCoordinates(distribution.Distribution);
                var polygon = drawing.CreatePolygonDrawing(distributionPoints);
                SetPolygonProperties(polygon);
                ShowPopUp(polygon, drawing, popup);

                Children.Add(polygon);
                Children.Add(popup);
            }
        }

        private IList<PointD> GetDistributionCoordinates(IList<Distribution> distribution)
        {
            var result = distribution.Select(e => e.ConvertToPointD()).OrderBy(f => f.Y).ToList();
            result.Add(new PointD(0, distribution.Max(e => e.Y)));
            result.Add(new PointD(0, distribution.Min(e => e.Y)));
            return result;
        }

        private IList<PointD> SectionBoxCoordinates()
        {
            var maxX = DistributionData.Max(e => e.SectionMaxX);
            var minX = DistributionData.Min(e => e.SectionMinX);
            var maxY = DistributionData.Max(e => e.SectionMaxY);
            var minY = DistributionData.Min(e => e.SectionMinY);
            var result = new List<PointD>()
            {
                new PointD(maxX,minY),
                new PointD(minX,maxY),
            };
            return result;
        }

        private void SetPolygonProperties(Polygon polygon)
        {
            var color1 = Color.FromRgb(158, 241, 14);
            var color2 = Color.FromRgb(21, 157, 24);
            var brush = new LinearGradientBrush(color1, color2, 90);

            polygon.Fill = brush;
            polygon.Name = "polygon";
        }

        private void ShowPopUp(Polygon polygon, PolygonDrawing drawing, DistributionPopup popup)
        {
            polygon.MouseLeave += (s, e) => popup.IsOpen = false;
            polygon.MouseMove += (s, e) =>
            {
                if (!popup.IsOpen)
                    popup.IsOpen = true;
                Point currentPosition = e.GetPosition(polygon);
                popup.HorizontalOffset = currentPosition.X - 50;
                popup.VerticalOffset = currentPosition.Y - 30;
                popup.UpdatePoint(drawing.TransformCoordinatesFromCentreOfGrid(currentPosition));
            };
            popup.PlacementTarget = polygon;
        }
    }
}