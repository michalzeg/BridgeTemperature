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
    public class BoxGriderWindowViewModel : ViewModelBase
    {
        public SectionPropertiesViewModel ConcretePropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }

        public BoxGriderWindowViewModel()
        {
            ConcretePropertiesVM = new SectionPropertiesViewModel();
            ConcretePropertiesVM.Materials = MaterialProvider.GetConcreteMaterials().ToList();

            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);

            _tf2 = 0.2;
            _hw = 1.2;
            _tf1 = 0.2;
            _bf1 = 2;
            _bf2 = 0.8;
            _tw = 0.2;
            _dt1 = 50;
            _dt2 = 10;
            _h1 = 0.2;
            _h2 = 0.2;
            _h3 = 0.2;
            _h4 = 0.3;
            _dt3 = 10;
            _dt4 = 40;

            _concreteGirder = new BoxGirder(Tf1, Hw, Tf2, Tw, Bf1, Bf2, DT1, DT2, DT3, DT4, H1, H2, H3, H4);
            updateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var concreteSection = new Section(_concreteGirder.GetIGirderCoordinates(), SectionType.Concrete,
                ConcretePropertiesVM.ModulusOfElasticity, ConcretePropertiesVM.ThermalCoefficient,
                _concreteGirder.GetIGirderDistribution());

            Messenger.Default.Send<ISection>(concreteSection);
        }

        private readonly BoxGirder _concreteGirder;

        private void updateDrawings()
        {
            var sectionCoordinates = _concreteGirder.GetIGirderCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Concrete } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData
            {
                Distribution = _concreteGirder.GetIGirderDistribution(),
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
                    _concreteGirder.Tf1 = value;
                    updateDrawings();
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
                    _concreteGirder.Hw = value;
                    updateDrawings();
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
                    _concreteGirder.Tf2 = value;
                    updateDrawings();
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
                    _concreteGirder.Tw = value;
                    updateDrawings();
                }
            }
        }

        private double _bf1;

        public double Bf1
        {
            get { return _bf1; }
            set
            {
                if (value != _bf1)
                {
                    _bf1 = value;
                    _concreteGirder.Bf1 = value;
                    updateDrawings();
                }
            }
        }

        private double _bf2;

        public double Bf2
        {
            get { return _bf2; }
            set
            {
                if (value != _bf2)
                {
                    _bf2 = value;
                    _concreteGirder.Bf2 = value;
                    updateDrawings();
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
                    _concreteGirder.DT1 = value;
                    updateDrawings();
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
                    _concreteGirder.DT2 = value;
                    updateDrawings();
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
                    _concreteGirder.H1 = value;
                    updateDrawings();
                }
            }
        }

        private double _h2;

        public double H2
        {
            get { return _h2; }
            set
            {
                if (value != _h2)
                {
                    _h2 = value;
                    _concreteGirder.H2 = value;
                    updateDrawings();
                }
            }
        }

        private double _dt3;

        public double DT3
        {
            get { return _dt3; }
            set
            {
                if (value != _dt3)
                {
                    _dt3 = value;
                    _concreteGirder.DT3 = value;
                    updateDrawings();
                }
            }
        }

        private double _dt4;

        public double DT4
        {
            get { return _dt4; }
            set
            {
                if (value != _dt4)
                {
                    _dt4 = value;
                    _concreteGirder.DT4 = value;
                    updateDrawings();
                }
            }
        }

        private double _h3;

        public double H3
        {
            get { return _h3; }
            set
            {
                if (value != _h3)
                {
                    _h3 = value;
                    _concreteGirder.H3 = value;
                    updateDrawings();
                }
            }
        }

        private double _h4;

        public double H4
        {
            get { return _h4; }
            set
            {
                if (value != _h4)
                {
                    _h4 = value;
                    _concreteGirder.H4 = value;
                    updateDrawings();
                }
            }
        }
    }
}