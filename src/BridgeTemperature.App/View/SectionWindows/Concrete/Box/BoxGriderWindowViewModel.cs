using BridgeTemperature.DistributionOperations;
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
    public class BoxGriderWindowViewModel : ViewModelBase
    {
        public SectionPropertiesViewModel ConcretePropertiesVM { get; private set; }
        public RelayCommand Apply { get; private set; }

        public BoxGriderWindowViewModel()
        {
            ConcretePropertiesVM = new SectionPropertiesViewModel();
            ConcretePropertiesVM.Materials = MaterialProperties.MaterialProvider.GetConcreteMaterials().ToList();

            Section = new List<SectionDrawingData>();
            TempDistribution = new List<DistributionDrawingData>();
            Apply = new RelayCommand(apply);

            tf2 = 0.2;
            hw = 1.2;
            tf1 = 0.2;
            bf1 = 2;
            bf2 = 0.8;
            tw = 0.2;
            dt1 = 50;
            dt2 = 10;
            h1 = 0.2;
            h2 = 0.2;
            h3 = 0.2;
            h4 = 0.3;
            dt3 = 10;
            dt4 = 40;

            concreteGirder = new BoxGirder(Tf1, Hw, Tf2, Tw, Bf1, Bf2, DT1, DT2, DT3, DT4, H1, H2, H3, H4);
            updateDrawings();
        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> TempDistribution { get; set; }

        private void apply()
        {
            var concreteSection = new Section(concreteGirder.GetIGirderCoordinates(), SectionType.Concrete,
                ConcretePropertiesVM.ModulusOfElasticity, ConcretePropertiesVM.ThermalCoefficient,
                concreteGirder.GetIGirderDistribution());

            Messenger.Default.Send<ISection>(concreteSection);
        }

        private BoxGirder concreteGirder;

        private void updateDrawings()
        {
            var sectionCoordinates = concreteGirder.GetIGirderCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Concrete } };
            Section = section;
            RaisePropertyChanged(() => Section);

            var distributionData = new DistributionDrawingData
            {
                Distribution = concreteGirder.GetIGirderDistribution(),
                SectionMaxY = sectionCoordinates.Max(e => e.Y),
                SectionMinY = sectionCoordinates.Min(e => e.Y),
                SectionMaxX = sectionCoordinates.Max(e => e.X),
                SectionMinX = sectionCoordinates.Min(e => e.X)
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
                    concreteGirder.Tf1 = value;
                    updateDrawings();
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
                    concreteGirder.Hw = value;
                    updateDrawings();
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
                    concreteGirder.Tf2 = value;
                    updateDrawings();
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
                    concreteGirder.Tw = value;
                    updateDrawings();
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
                    concreteGirder.Bf1 = value;
                    updateDrawings();
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
                    concreteGirder.Bf2 = value;
                    updateDrawings();
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
                    concreteGirder.DT1 = value;
                    updateDrawings();
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
                    concreteGirder.DT2 = value;
                    updateDrawings();
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
                    concreteGirder.H1 = value;
                    updateDrawings();
                }
            }
        }

        private double h2;

        public double H2
        {
            get { return h2; }
            set
            {
                if (value != h2)
                {
                    h2 = value;
                    concreteGirder.H2 = value;
                    updateDrawings();
                }
            }
        }

        private double dt3;

        public double DT3
        {
            get { return dt3; }
            set
            {
                if (value != dt3)
                {
                    dt3 = value;
                    concreteGirder.DT3 = value;
                    updateDrawings();
                }
            }
        }

        private double dt4;

        public double DT4
        {
            get { return dt4; }
            set
            {
                if (value != dt4)
                {
                    dt4 = value;
                    concreteGirder.DT4 = value;
                    updateDrawings();
                }
            }
        }

        private double h3;

        public double H3
        {
            get { return h3; }
            set
            {
                if (value != h3)
                {
                    h3 = value;
                    concreteGirder.H3 = value;
                    updateDrawings();
                }
            }
        }

        private double h4;

        public double H4
        {
            get { return h4; }
            set
            {
                if (value != h4)
                {
                    h4 = value;
                    concreteGirder.H4 = value;
                    updateDrawings();
                }
            }
        }
    }
}