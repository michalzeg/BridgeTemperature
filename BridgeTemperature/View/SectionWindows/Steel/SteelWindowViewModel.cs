﻿using BridgeTemperature.Drawing;
using BridgeTemperature.Helpers;
using BridgeTemperature.View.ViewClasses;
using GalaSoft.MvvmLight;
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

        public SteelWindowViewModel()
        {
            SectionPropertiesVM = new SectionPropertiesViewModel();
            SectionPropertiesVM.Materials = MaterialProperties.MaterialOperations.GetSteelMaterials().ToList();
            Section = new List<SectionDrawingData>();
            Distribution = new List<DistributionDrawingData>();

            tf2 = 0.02;
            hw = 1;
            tf1 = 0.02;
            bf = 0.3;
            h1 = 0.2;
            dt1 = 20;

            steelPlateGirder = new SteelPlateGirder(Tf1, Hw, Tf2, Bf, Tw, H1, DT1);

        }

        public IList<SectionDrawingData> Section { get; set; }
        public IList<DistributionDrawingData> Distribution { get; set; }


        private SteelPlateGirder steelPlateGirder;
        private void updateDrawings()
        {
            var sectionCoordinates = steelPlateGirder.GetCoordinates();
            var section = new List<SectionDrawingData>()
            { new SectionDrawingData(){ Coordinates = sectionCoordinates, Type = SectionType.Fill } };
            Section = section;

            var distribution = new DistributionDrawingData();
            distribution.Distribution = steelPlateGirder.GetDistribution();
            distribution.SectionMaxY = sectionCoordinates.Max(e => e.Y);
            distribution.SectionMinY = sectionCoordinates.Min(e => e.Y);
            distribution.SectionMaxX = sectionCoordinates.Max(e => e.X);
            distribution.SectionMinX = sectionCoordinates.Min(e => e.X);
            Distribution = new List<DistributionDrawingData>() { distribution };

            RaisePropertyChanged(() => Section);
            RaisePropertyChanged(() => Distribution);

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
                    updateDrawings();
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
                    updateDrawings();
                }
            }
        }
    }
}
