
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using BridgeTemperature.Helpers;
using BridgeTemperature.DistributionOperations;
using System.Windows.Controls.Primitives;
using BridgeTemperature.Extensions;
using System.Windows.Documents;

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
    public class SectionDrawingData
    {
        public IList<PointD> Coordinates;
        public SectionType Type;
    }

    public class ScaleCalculator
    {
        public double ScaleY { get; set; }
        public double ScaleX { get; set; }
        public PointD Centre { get; set; }
        public double MaxY { get; private set; }
        public double MinY { get; private set; }


        //private IList<IList<PointD>> perimeters;
        public Func<double> CanvasActualWidth;
        public Func<double> CanvasActualHeight;
  

        public ScaleCalculator(Func<double> actualWidth, Func<double> actualHeight)
        {
            //this.perimeters = new List<IList<PointD>>();
            this.CanvasActualHeight = actualHeight;
            this.CanvasActualWidth = actualWidth;
            this.Centre = new PointD(0, 0);

        }

        public void UpdateProperties(IList<IList<PointD>> perimeters)
        {
            if (perimeters == null || perimeters.Count == 0)
                return;

            var xMax = perimeters.Max(e => e.Max(g => g.X));
            var xMin = perimeters.Min(e => e.Min(g => g.X));
            var yMax = perimeters.Max(e => e.Max(g => g.Y));
            var yMin = perimeters.Min(e => e.Min(g => g.Y));

            this.MaxY = yMax;
            this.MinY = yMin;

            var drawingWidth = xMax - xMin;
            var drawingHeight = yMax - yMin;

            this.Centre = new PointD(drawingWidth / 2 + xMin, drawingHeight / 2 + yMin);
            var scaleX = this.CanvasActualWidth() / drawingWidth;
            var scaleY = this.CanvasActualHeight() / drawingHeight;

            calculateScale(scaleX,scaleY);

        }
        protected virtual void calculateScale(double scaleX,double scaleY)
        {
            
            var scale = Math.Min(scaleX, scaleY);
            this.ScaleY = scale;
            this.ScaleX = scale;
        }
    }

    public class DistributionScaleCalculator:ScaleCalculator
    {
        public DistributionScaleCalculator(Func<double> actualWidth,Func<double> actualHeight)
            :base(actualWidth,actualHeight)
        {

        }

        protected override void calculateScale(double scaleX, double scaleY)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
        }
    }

    
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

    
    public class PolygonDrawing:DrawingBase
    {

        public PolygonDrawing(ScaleCalculator scaleProperties):base(scaleProperties)
        {
        }


        public Polygon CreatePolygonDrawing(IList<PointD> section)
        {
            if (section == null || section.Count == 0)
                return null;

            var polygon = new Polygon();
            var transferedCoordinates = base.transformCoordinatesToCentreOfGrid(section);
            foreach (var point in transferedCoordinates)
            {
                polygon.Points.Add(new Point(point.X, point.Y));
            }
            return polygon;
        }
    }

  
    public class DistributionDrawing:Grid
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
        //private DistributionPopup popup;

        public DistributionDrawing()
        {
            distributionScaleCalculator = new DistributionScaleCalculator(() => ActualWidth,()=>ActualHeight);
            sectionScaleCalculator = new ScaleCalculator(() => SectionCanvasWidth, () => SectionCanvasHeight);
            //popup = new DistributionPopup();
            
        }
        public void RefreshDrawing()
        {
            if (DistributionData == null || DistributionData.Count == 0) 
                return;

            Children.Clear();
            var sectionCoordinates = new List<IList<PointD>>();
            sectionCoordinates.Add(sectionBoxCoordinates());
            sectionScaleCalculator.UpdateProperties(sectionCoordinates);


            //REFACTOR THAT METHOD LATER
            var distributionCoordinates = new List<IList<PointD>>();
            foreach (var distribution in DistributionData)
            {
                if (distribution.Distribution == null || distribution.Distribution.Count == 0)
                    continue;
                var distributionPoints = this.distributionCoordinates(distribution.Distribution);
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
                var distributionPoints = this.distributionCoordinates(distribution.Distribution);
                var polygon = drawing.CreatePolygonDrawing(distributionPoints);
                setPolygonProperties(polygon);
                showPopUp(polygon, drawing, popup);

                Children.Add(polygon);
                Children.Add(popup);
            }
        }
        private IList<PointD> distributionCoordinates(IList<Distribution> distribution)
        {
            var result = distribution.Select(e => e.ConvertToPointD()).OrderBy(f => f.Y).ToList();
            result.Add(new PointD(0,distribution.Max(e => e.Y)));
            result.Add(new PointD(0,distribution.Min(e => e.Y)));
            return result;
        }
        private IList<PointD> sectionBoxCoordinates()
        {
            var maxX = DistributionData.Max(e => e.SectionMaxX);
            var minX = DistributionData.Min(e => e.SectionMinX);
            var maxY = DistributionData.Max(e => e.SectionMaxY);
            var minY = DistributionData.Min(e => e.SectionMinY);


            var result = new List<PointD>()
            {
                //new PointD(distributionData.SectionMaxX,distributionData.SectionMinY),
                //new PointD(distributionData.SectionMinX,distributionData.SectionMaxY)
                new PointD(maxX,minY),
                new PointD(minX,maxY),
            };
            return result;
        }

        private void setLineProperties(Line line)
        {
            
        }
        private void setPolygonProperties(Polygon polygon)
        {
            var brush = new LinearGradientBrush(Brushes.Crimson.Color, Brushes.Red.Color, 90);
            //polygon.Stroke = Brushes.DarkRed;
            //polygon.StrokeThickness = 2;
            polygon.Fill = brush;
            polygon.Name = "polygon";
            
        }
        private void showPopUp(Polygon polygon,PolygonDrawing drawing,DistributionPopup popup)
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
    public class SectionCanvas:Grid
    {
        static SectionCanvas()
        {
            var metaData = new FrameworkPropertyMetadata(new PropertyChangedCallback (OnSectionChanged));
            metaData.AffectsRender = true;
            metaData.BindsTwoWayByDefault = true;
            
            SectionsDependencyProperty = DependencyProperty.Register("Sections", typeof(IList<SectionDrawingData>), typeof(SectionCanvas),metaData);
            
        }

        public static void OnSectionChanged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as SectionCanvas;
            canvas.Sections = e.NewValue as IList<SectionDrawingData>;
            canvas.RefreshDrawing();
        }

        public static readonly DependencyProperty SectionsDependencyProperty;
        public IList<SectionDrawingData> Sections
        {
            get
            {
                return GetValue(SectionsDependencyProperty) as IList<SectionDrawingData>;
            }
            set
            {
                SetValue(SectionsDependencyProperty, value);

            }
        }
        private ScaleCalculator scaleCalculator;

        public SectionCanvas():base()
        {
            
            scaleCalculator = new ScaleCalculator(() => ActualWidth, () => ActualHeight);
        }
        protected void clearDrawing()
        {
            Children.Clear();
        }
        public void RefreshDrawing()
        {

            PolygonDrawing drawing = new PolygonDrawing(scaleCalculator);
            this.scaleCalculator.UpdateProperties(Sections.Select(e => e.Coordinates).ToList());
            this.Children.Clear();
            foreach (var section in Sections)
            {
                
                var polygon = drawing.CreatePolygonDrawing(section.Coordinates);
                setPolygonProperties(section.Type, polygon);
                Children.Add(polygon);
            }

        }
        protected void setPolygonProperties(SectionType type, Polygon polygon)
        {

            var brush = (type == SectionType.Fill) ? fillBrush() : voidBrush();
            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;

            polygon.Fill = brush;//(type == SectionType.Fill) ? Brushes.Gray : Brushes.Green;
        }

        protected Brush fillBrush()
        {

            return new LinearGradientBrush(Brushes.LightGray.Color, Brushes.Gray.Color, 90);

        } 
        protected Brush voidBrush()
        {
            return Brushes.Bisque;
        }

    }

    public class DistributionPopup:Popup
    {
        
        private TextBlock textBlock;

        public DistributionPopup():base()
        {
            this.AllowsTransparency = true;
            this.Placement = PlacementMode.Relative;

            textBlock = new TextBlock();
            this.Child = textBlock;
            textBlock.Foreground = Brushes.Black;
            
        }
        
        public void UpdatePoint(PointD point)
        {
            textBlock.Inlines.Clear();
            
            textBlock.Inlines.Add(new Bold(new Run(string.Format("Y: {0}\n", point.Y.Round(2)))));
            textBlock.Inlines.Add(new Bold(new Run(string.Format("Value: {0}", point.X.Round(2)))));
        }
    }

}
