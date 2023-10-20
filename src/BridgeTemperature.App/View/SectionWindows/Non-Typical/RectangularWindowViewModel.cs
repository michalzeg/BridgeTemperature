using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Calculations.Sections;
using BridgeTemperature.Shared.Materials;
using BridgeTemperature.Shared.Sections;
using BridgeTemperature.Drawing;
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
    public class RectangularWindowViewModel : ViewModelBase
    {
        public SectionPropertiesViewModel SectionPropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }

        public RectangularWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Materials = MaterialProvider.GetAllMaterials().ToList();
            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);
            _h = 0.2;
            _b = 1;
            _x = 0.5;
            _y = 0.1;
            _dt1 = 30;
            _dt2 = 20;

            _rectangularGirder = new RectangularGirder(B, H, X, Y, DT1, DT2);
            UpdateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var section = new Section(_rectangularGirder.GetCoordinates(), SectionType.Custom,
                SectionPropertiesVM.ModulusOfElasticity, SectionPropertiesVM.ThermalCoefficient,
                _rectangularGirder.GetTemperature());
            Messenger.Default.Send<ISection>(section);
        }

        private readonly RectangularGirder _rectangularGirder;

        private void UpdateDrawings()
        {
            var sectionCoordinates = _rectangularGirder.GetCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Custom } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData
            {
                Distribution = _rectangularGirder.GetTemperature(),
                SectionMaxY = sectionCoordinates.Max(e => e.Y),
                SectionMinY = sectionCoordinates.Min(e => e.Y),
                SectionMaxX = sectionCoordinates.Max(e => e.X),
                SectionMinX = sectionCoordinates.Min(e => e.X)
            };
            var distribution = new List<DistributionDrawingData>() { distributionData };
            TempDistribution = distribution;
            RaisePropertyChanged(() => TempDistribution);
        }

        private double _b;

        public double B
        {
            get { return _b; }
            set
            {
                if (value != _b)
                {
                    _b = value;
                    _rectangularGirder.B = value;
                    UpdateDrawings();
                }
            }
        }

        private double _h;

        public double H
        {
            get { return _h; }
            set
            {
                if (value != _h)
                {
                    _h = value;
                    _rectangularGirder.H = value;
                    UpdateDrawings();
                }
            }
        }

        private double _x;

        public double X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    _rectangularGirder.X = value;
                    UpdateDrawings();
                }
            }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set
            {
                if (value != _y)
                {
                    _y = value;
                    _rectangularGirder.Y = value;
                    UpdateDrawings();
                }
            }
        }

        private double _dt1;

        public double DT1
        {
            get { return _dt1; }
            set
            {
                if (value != _dt1)
                {
                    _dt1 = value;
                    _rectangularGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _dt2;

        public double DT2
        {
            get { return _dt2; }
            set
            {
                if (value != _dt2)
                {
                    _dt2 = value;
                    _rectangularGirder.DT2 = value;
                    UpdateDrawings();
                }
            }
        }
    }
}