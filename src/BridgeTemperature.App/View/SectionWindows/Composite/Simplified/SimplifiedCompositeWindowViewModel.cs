using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Calculations.Sections;
using BridgeTemperature.Common.Materials;
using BridgeTemperature.Common.Sections;
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

            tf2 = 0.02;
            hw = 1;
            tf1 = 0.02;
            bf1 = 0.3;
            bf2 = 0.2;
            bp = 1;
            hp = 0.3;
            tw = 0.02;
            hp = 0.2;
            dt1 = 20;

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

        private double tf1;

        public double Tf1
        {
            get { return tf1; }
            set
            {
                if (value != tf1)
                {
                    tf1 = value;
                    compositeGirder.Tf1 = value;
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
                    compositeGirder.Hw = value;
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
                    compositeGirder.Tf2 = value;
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
                    compositeGirder.Tw = value;
                    UpdateDrawings();
                }
            }
        }

        private double bf1;

        public double Bf1
        {
            get { return bf1; }
            set
            {
                if (value != bf1)
                {
                    bf1 = value;
                    compositeGirder.Bf1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double bf2;

        public double Bf2
        {
            get { return bf2; }
            set
            {
                if (value != bf2)
                {
                    bf2 = value;
                    compositeGirder.Bf2 = value;
                    UpdateDrawings();
                }
            }
        }

        private double bp;

        public double Bp
        {
            get { return bp; }
            set
            {
                if (value != bp)
                {
                    bp = value;
                    compositeGirder.Bp = value;
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
                    compositeGirder.DT1 = value;
                    UpdateDrawings();
                }
            }
        }

        private double hp;

        public double Hp
        {
            get { return hp; }
            set
            {
                if (value != hp)
                {
                    hp = value;
                    compositeGirder.Hp = value;
                    UpdateDrawings();
                }
            }
        }
    }
}