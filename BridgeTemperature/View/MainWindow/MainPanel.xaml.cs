using BridgeTemperature.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BridgeTemperature.View
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : UserControl
    {
        public MainPanel()
        {
            InitializeComponent();
            this.sectionCanvas.SizeChanged += (a, e) => this.sectionCanvas.RefreshDrawing();
            this.bendingDistributionDrawing.SizeChanged += (a, e) => this.bendingDistributionDrawing.RefreshDrawing();
            this.externalDistributionDrawing.SizeChanged += (a, e) => this.externalDistributionDrawing.RefreshDrawing();
            this.uniformDistributionDrawing.SizeChanged += (a, e) => this.uniformDistributionDrawing.RefreshDrawing();
            this.selfEqulibratingDistributionDrawing.SizeChanged += (a, e) => this.selfEqulibratingDistributionDrawing.RefreshDrawing();

            this.SizeChanged += (a, e) => sectionCanvas.RefreshDrawing();
        }
    }
}
