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
    public class SimplifiedCompositeWindowViewModel : ViewModelBase
    {
        public SectionPropertiesViewModel SteelPropertiesVM { get; private set; }
        public SectionPropertiesViewModel ConcretePropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }

        public SimplifiedCompositeWindowViewModel()
        {
            SteelPropertiesVM = new SectionPropertiesViewModel();
            SteelPropertiesVM.Materials = MaterialProvider.GetSteelMaterials().ToList();
            ConcretePropertiesVM = new SectionPropertiesViewModel();
            ConcretePropertiesVM.Materials = MaterialProvider.GetConcreteMaterials().ToList();

            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);

            _tf2 = 0.02;
            _hw = 1;
            _tf1 = 0.02;
            _bf1 = 0.3;
            _bf2 = 0.2;
            _bp = 1;
            _hp = 0.3;
            _tw = 0.02;
            _hp = 0.2;
            _dt1 = 20;

            compositeGirder = new SimplifiedCompositeGirder(Tf1, Hw, Tf2, Bf1, Bf2, Tw, Hp, Bp, DT1);
            UpdateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var steelSection = new Section(compositeGirder.GetPlateGirderCoordinates(), SectionType.Steel,
                SteelPropertiesVM.ModulusOfElasticity, SteelPropertiesVM.ThermalCoefficient,
                compositeGirder.GetPlateGirderTemperature());
            Messenger.Default.Send<ISection>(steelSection);

            var concreteSection = new Section(compositeGirder.GetSlabCoordinates(), SectionType.Concrete,
                ConcretePropertiesVM.ModulusOfElasticity, ConcretePropertiesVM.ThermalCoefficient,
                compositeGirder.GetSlabTemperature());

            Messenger.Default.Send<ISection>(concreteSection);
        }

        private SimplifiedCompositeGirder compositeGirder;

        private void UpdateDrawings()
        {
            var steelSection = compositeGirder.GetPlateGirderCoordinates();
            var concreteSection = compositeGirder.GetSlabCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = steelSection, Type = SectionType.Steel },
            new SectionDrawingData() {Coordinates = concreteSection,Type= SectionType.Concrete } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData
            {
                Distribution = compositeGirder.GetSlabTemperature(),
                SectionMaxY = compositeGirder.MaxY,
                SectionMinY = compositeGirder.MinY,
                SectionMaxX = compositeGirder.MaxX,
                SectionMinX = compositeGirder.MinX
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
                    compositeGirder.Tf1 = value;
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
                    compositeGirder.Hw = value;
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
                    compositeGirder.Tf2 = value;
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
                    compositeGirder.Tw = value;
                    UpdateDrawings();
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
                    compositeGirder.Bf1 = value;
                    UpdateDrawings();
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
                    compositeGirder.Bf2 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _bp;

        public double Bp
        {
            get { return _bp; }
            set
            {
                if (value != _bp)
                {
                    _bp = value;
                    compositeGirder.Bp = value;
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
                    compositeGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double _hp;

        public double Hp
        {
            get { return _hp; }
            set
            {
                if (value != _hp)
                {
                    _hp = value;
                    compositeGirder.Hp = value;
                    UpdateDrawings();
                }
            }
        }
    }
}