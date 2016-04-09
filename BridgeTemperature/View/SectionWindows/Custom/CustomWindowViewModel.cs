using BridgeTemperature.Helpers;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Common;
using System.Windows.Controls;
using BridgeTemperature.DistributionOperations;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using BridgeTemperature.Drawing;
using BridgeTemperature.Sections;
using GalaSoft.MvvmLight.Messaging;

namespace BridgeTemperature.ViewModel
{
    public class CustomWindowViewModel : ViewModelBase
    {

        public SectionPropertiesViewModel SectionPropertiesVM { get; set; }
        public RelayCommand Apply { get; private set; }
        public RelayCommand PointsUpdated { get; private set; }
        public RelayCommand TemperatureUpdated { get; private set; }


        public CustomWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Update += this.pointUpdated;

            PointsUpdated = new RelayCommand(pointUpdated);
            TemperatureUpdated = new RelayCommand(distributionUpdated);


            this.Points = new ObservableCollection<PointD>();
            this.Temperature = new ObservableCollection<Distribution>();

            Section = new ObservableCollection<SectionDrawingData>();

            Apply = new RelayCommand(apply);
        }
   
        public ObservableCollection<PointD> Points { get; set; }
        public ObservableCollection<Distribution> Temperature {get;set;}
        public IList<SectionDrawingData> Section { get; set; }
        public DistributionDrawingData Distribution { get; set; }


        private void apply()
        {
            var section = new Section(Points, SectionPropertiesVM.Type,
                SectionPropertiesVM.ModulusOfElasticity, SectionPropertiesVM.ThermalCoefficient,
                Temperature);
            Messenger.Default.Send<ISection>(section);

        }
        private void pointUpdated()
        {
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = Points, Type = SectionPropertiesVM.Type } };
            Section = section;
            RaisePropertyChanged(() => Section);
            RaisePropertyChanged(() => Distribution);
        }
        private void distributionUpdated()
        {
            var distribution = new DistributionDrawingData();
            distribution.Distribution = Temperature;
            distribution.SectionMaxY = Points.Max(e => e.Y);
            distribution.SectionMinY = Points.Min(e => e.Y);
            distribution.SectionMaxX = Points.Max(e => e.X);
            distribution.SectionMinX = Points.Min(e => e.X);
            Distribution = distribution;
            
            RaisePropertyChanged(() => Distribution);
        }

    }
}
