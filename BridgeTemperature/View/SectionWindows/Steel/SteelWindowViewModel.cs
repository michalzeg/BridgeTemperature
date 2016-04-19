using BridgeTemperature.Drawing;
using BridgeTemperature.Helpers;
using BridgeTemperature.Sections;
using BridgeTemperature.View.ViewClasses;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTemperature.ViewModel
{
    public class SteelWindowViewModel:ViewModelBase
    {
        public SectionPropertiesViewModel SectionPropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }
        public SteelWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Materials = MaterialProperties.MaterialOperations.GetSteelMaterials().ToList();
            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);

            tf2 = 0.02;
            hw = 1;
            tf1 = 0.02;
            bf = 0.3;
            tw = 0.02;
            h1 = 0.2;
            dt1 = 20;

            steelPlateGirder = new SteelPlateGirder(Tf1, Hw, Tf2, Bf, Tw, H1, DT1);
            UpdateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var section = new Section(steelPlateGirder.GetCoordinates(), SectionType.Steel,
                SectionPropertiesVM.ModulusOfElasticity, SectionPropertiesVM.ThermalCoefficient,
                steelPlateGirder.GetTemperature());
            Messenger.Default.Send<ISection>(section);


        }
        private SteelPlateGirder steelPlateGirder;
        private void UpdateDrawings()
        {
            var sectionCoordinates = steelPlateGirder.GetCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Steel } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData();
            distributionData.Distribution = steelPlateGirder.GetTemperature();
            distributionData.SectionMaxY = sectionCoordinates.Max(e => e.Y);
            distributionData.SectionMinY = sectionCoordinates.Min(e => e.Y);
            distributionData.SectionMaxX = sectionCoordinates.Max(e => e.X);
            distributionData.SectionMinX = sectionCoordinates.Min(e => e.X);
            var distribution = new List<DistributionDrawingData>() { distributionData }; 
            TempDistribution = distribution;

            
            RaisePropertyChanged(() => TempDistribution);

        }
        private double tf1;
        public double Tf1
        {
            get { return tf1; }
            set
            {
                if (value!=tf1)
                {
                    tf1 = value;
                    steelPlateGirder.Tf1 = value;
                    UpdateDrawings();
                }
            }
        }
        private double hw;
        public double Hw
        {
            get { return hw; }
            set
            {
                if (value != hw)
                {
                    hw = value;
                    steelPlateGirder.Hw = value;
                    UpdateDrawings();
                }
            }
        }
        private double tf2;
        public double Tf2
        {
            get { return tf2; }
            set
            {
                if (value != tf2)
                {
                    tf2 = value;
                    steelPlateGirder.Tf2 = value;
                    UpdateDrawings();
                }
            }
        }
        private double tw;
        public double Tw
        {
            get { return tw; }
            set
            {
                if (value != tw)
                {
                    tw = value;
                    steelPlateGirder.Tw = value;
                    UpdateDrawings();
                }
            }
        }
        private double bf;
        public double Bf
        {
            get { return bf; }
            set
            {
                if (value != bf)
                {
                    bf = value;
                    steelPlateGirder.Bf = value;
                    UpdateDrawings();
                }
            }
        }
        private double dt1;
        public double DT1
        {
            get { return dt1; }
            set
            {
                if (value != dt1)
                {
                    dt1 = value;
                    steelPlateGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }
        private double h1;
        public double H1
        {
            get { return h1; }
            set
            {
                if (value != h1)
                {
                    h1 = value;
                    steelPlateGirder.H1 = value;
                    UpdateDrawings();
                }
            }
        }
    }
}
