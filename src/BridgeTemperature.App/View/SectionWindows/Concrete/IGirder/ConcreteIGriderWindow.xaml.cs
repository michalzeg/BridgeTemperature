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
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Ioc;
using BridgeTemperature.ViewModel;
using BridgeTemperature.Helpers;
using BridgeTemperature.DistributionOperations;
using GalaSoft.MvvmLight.Messaging;

namespace BridgeTemperature.View
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class ConcreteIGirderWindow : Window
    {
        public ConcreteIGirderWindow()
        {
            InitializeComponent();
            //var viewModel = new CustomWindowViewModel();
            //this.DataContext = viewModel;

            //this.sectionCanvas.SizeChanged += (a, e) => this.sectionCanvas.RefreshDrawing();
            //this.distributionDrawing.SizeChanged += (a, e) => this.distributionDrawing.RefreshDrawing();

            this.Activated += (a, e) => this.distributionDrawing.RefreshDrawing();
            this.SizeChanged += (a, e) =>
            {
                this.sectionCanvas.RefreshDrawing();
                this.distributionDrawing.RefreshDrawing();
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}