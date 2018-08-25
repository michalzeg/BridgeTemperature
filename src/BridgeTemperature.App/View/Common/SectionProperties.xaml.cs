using BridgeTemperature.ViewModel;
using GalaSoft.MvvmLight.Ioc;
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
    /// Interaction logic for SectionProperties.xaml
    /// </summary>
    public partial class SectionProperties : UserControl
    {
        public SectionProperties()
        {
            InitializeComponent();

            this.comboMaterials.SelectedIndex = 0;
        }
    }
}