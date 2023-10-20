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
    public class SteelWindowViewModel : ViewModelBase
    {
        public SectionPropertiesViewModel SectionPropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }

        public SteelWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Materials = MaterialProvider.GetSteelMaterials().ToList();
            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);

            _tf2 = 0.02;
            _hw = 1;
            _tf1 = 0.02;
            _bf = 0.3;
            _tw = 0.02;
            _h1 = 0.2;
            _dt1 = 20;

            _steelPlateGirder = new SteelPlateGirder(Tf1, Hw, Tf2, Bf, Tw, H1, DT1);
            UpdateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var section = new Section(_steelPlateGirder.GetCoordinates(), SectionType.Steel,
                SectionPropertiesVM.ModulusOfElasticity, SectionPropertiesVM.ThermalCoefficient,
                _steelPlateGirder.GetTemperature());
            Messenger.Default.Send<ISection>(section);
        }

        private readonly SteelPlateGirder _steelPlateGirder;

        private void UpdateDrawings()
        {
            var sectionCoordinates = _steelPlateGirder.GetCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Steel } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData
            {
                Distribution = _steelPlateGirder.GetTemperature(),
                SectionMaxY = sectionCoordinates.Max(e => e.Y),
                SectionMinY = sectionCoordinates.Min(e => e.Y),
                SectionMaxX = sectionCoordinates.Max(e => e.X),
                SectionMinX = sectionCoordinates.Min(e => e.X)
            };
            var distribution = new List<DistributionDrawingData>() { distributionData };
            TempDistribution = distribution;
            RaisePropertyChanged(() => TempDistribution);
        }

        private double _tf1;

        public double Tf1
        {
            get { return _tf1; }
            set
            {
                if (value != _tf1)
                {
                    _tf1 = value;
                    _steelPlateGirder.Tf1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _hw;

        public double Hw
        {
            get { return _hw; }
            set
            {
                if (value != _hw)
                {
                    _hw = value;
                    _steelPlateGirder.Hw = value;
                    UpdateDrawings();
                }
            }
        }

        private double _tf2;

        public double Tf2
        {
            get { return _tf2; }
            set
            {
                if (value != _tf2)
                {
                    _tf2 = value;
                    _steelPlateGirder.Tf2 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _tw;

        public double Tw
        {
            get { return _tw; }
            set
            {
                if (value != _tw)
                {
                    _tw = value;
                    _steelPlateGirder.Tw = value;
                    UpdateDrawings();
                }
            }
        }

        private double _bf;

        public double Bf
        {
            get { return _bf; }
            set
            {
                if (value != _bf)
                {
                    _bf = value;
                    _steelPlateGirder.Bf = value;
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
                    _steelPlateGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _h1;

        public double H1
        {
            get { return _h1; }
            set
            {
                if (value != _h1)
                {
                    _h1 = value;
                    _steelPlateGirder.H1 = value;
                    UpdateDrawings();
                }
            }
        }
    }
}