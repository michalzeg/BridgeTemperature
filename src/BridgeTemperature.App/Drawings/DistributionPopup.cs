using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using BridgeTemperature.Common.Geometry;
using BridgeTemperature.Common.Extensions;

namespace BridgeTemperature.Drawing
{
    public class DistributionPopup : Popup
    {
        private TextBlock textBlock;

        public DistributionPopup() : base()
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