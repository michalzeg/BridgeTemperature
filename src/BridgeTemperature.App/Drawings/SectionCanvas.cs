using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;
using BridgeTemperature.Shared.Sections;

namespace BridgeTemperature.Drawing
{
    public class SectionCanvas : Grid
    {
        static SectionCanvas()
        {
            var metaData = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSectionChanged));
            metaData.AffectsRender = true;
            metaData.BindsTwoWayByDefault = true;

            SectionsDependencyProperty = DependencyProperty.Register("Sections", typeof(IList<SectionDrawingData>), typeof(SectionCanvas), metaData);
        }

        public static void OnSectionChanged(DependencyObject @object, DependencyPropertyChangedEventArgs @event)
        {
            var canvas = @object as SectionCanvas;
            canvas.Sections = @event.NewValue as IList<SectionDrawingData>;
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

        public SectionCanvas() : base()
        {
            scaleCalculator = new ScaleCalculator(() => ActualWidth, () => ActualHeight);
        }

        protected void clearDrawing()
        {
            Children.Clear();
        }

        public void RefreshDrawing()
        {
            if (Sections == null || Sections.Count == 0)
            {
                Children.Clear();
                return;
            }
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
            Brush brush = GetBrush(type);

            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;

            polygon.Fill = brush;
        }

        private Brush GetBrush(SectionType type)
        {
            Brush brush = CustomBrush();
            if (type == SectionType.Concrete)
                brush = ConcreteBrush();
            else if (type == SectionType.Steel)
                brush = SteelBrush();
            else if (type == SectionType.Void)
                brush = VoidBrush();
            return brush;
        }

        protected Brush CustomBrush()
        {
            return new LinearGradientBrush(Color.FromRgb(4, 90, 2), Color.FromRgb(6, 26, 0), 90);
        }

        protected Brush SteelBrush()
        {
            return new LinearGradientBrush(Color.FromRgb(252, 49, 49), Color.FromRgb(136, 0, 32), 90);
        }

        protected Brush ConcreteBrush()
        {
            return new LinearGradientBrush(Color.FromRgb(48, 48, 48), Color.FromRgb(120, 120, 120), 90);
        }

        protected Brush VoidBrush()
        {
            return Brushes.Bisque;
        }
    }
}