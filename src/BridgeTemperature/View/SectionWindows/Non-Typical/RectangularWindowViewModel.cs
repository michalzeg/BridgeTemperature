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
    public class RectangularWindowViewModel:ViewModelBase
    {
        public SectionPropertiesViewModel SectionPropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }
        public RectangularWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Materials = MaterialProperties.MaterialProvider.GetAllMaterials().ToList();
            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);
            h = 0.2;
            b = 1;
            x = 0.5;
            y = 0.1;
            dt1 = 30;
            dt2 = 20;

            rectangularGirder = new RectangularGirder(B, H, X,Y, DT1, DT2);
            UpdateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var section = new Section(rectangularGirder.GetCoordinates(), SectionType.Custom,
                SectionPropertiesVM.ModulusOfElasticity, SectionPropertiesVM.ThermalCoefficient,
                rectangularGirder.GetTemperature());
            Messenger.Default.Send<ISection>(section);
        }
        private RectangularGirder rectangularGirder;
        private void UpdateDrawings()
        {
            var sectionCoordinates = rectangularGirder.GetCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Custom } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData();
            distributionData.Distribution = rectangularGirder.GetTemperature();
            distributionData.SectionMaxY = sectionCoordinates.Max(e => e.Y);
            distributionData.SectionMinY = sectionCoordinates.Min(e => e.Y);
            distributionData.SectionMaxX = sectionCoordinates.Max(e => e.X);
            distributionData.SectionMinX = sectionCoordinates.Min(e => e.X);
            var distribution = new List<DistributionDrawingData>() { distributionData }; 
            TempDistribution = distribution;
            RaisePropertyChanged(() => TempDistribution);

        }
        private double b;
        public double B
        {
            get { return b; }
            set
            {
                if (value!=b)
                {
                    b = value;
                    rectangularGirder.B = value;
                    UpdateDrawings();
                }
            }
        }
        private double h;
        public double H
        {
            get { return h; }
            set
            {
                if (value != h)
                {
                    h = value;
                    rectangularGirder.H = value;
                    UpdateDrawings();
                }
            }
        }
        private double x;
        public double X
        {
            get { return x; }
            set
            {
                if (value != x)
                {
                    x = value;
                    rectangularGirder.X = value;
                    UpdateDrawings();
                }
            }
        }
        private double y;
        public double Y
        {
            get { return y; }
            set
            {
                if (value != y)
                {
                    y = value;
                    rectangularGirder.Y = value;
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
                    rectangularGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }
        private double dt2;
        public double DT2
        {
            get { return dt2; }
            set
            {
                if (value != dt2)
                {
                    dt2 = value;
                    rectangularGirder.DT2 = value;
                    UpdateDrawings();
                }
            }
        }
    }
}
