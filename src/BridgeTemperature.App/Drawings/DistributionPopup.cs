using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Extensions;

namespace BridgeTemperature.Drawing
{
    public class DistributionPopup : Popup
    {
        private readonly TextBlock _textBlock;

        public DistributionPopup() : base()
        {
            AllowsTransparency = true;
            Placement = PlacementMode.Relative;

            _textBlock = new TextBlock();
            Child = _textBlock;
            _textBlock.Foreground = Brushes.Black;
        }

        public void UpdatePoint(PointD point)
        {
            _textBlock.Inlines.Clear();

            _textBlock.Inlines.Add(new Bold(new Run(string.Format("Y: {0}\n", point.Y.Round(2)))));
            _textBlock.Inlines.Add(new Bold(new Run(string.Format("Value: {0}", point.X.Round(2)))));
        }
    }
}