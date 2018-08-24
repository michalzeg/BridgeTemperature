using BridgeTemperature.Helpers;
using BridgeTemperature.MaterialProperties;
using BridgeTemperature.Sections;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BridgeTemperature.Drawing;

namespace BridgeTemperature.ViewModel
{
    public class SectionPropertiesViewModel : ViewModelBase
    {
        public IList<Material> Materials { get; set; }

        public SectionPropertiesViewModel()
        {
            Materials = MaterialProvider.GetAllMaterials().ToList();
            RaisePropertyChanged(() => Materials);
        }

        private Material selectedMaterial;
        public Material SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                if (value != selectedMaterial)
                {
                    selectedMaterial = value;
                    this.ModulusOfElasticity = value.E;
                    this.ThermalCoefficient = value.ThermalCoefficient;
                    RaisePropertyChanged(() => this.SelectedMaterial);
                }
            }
        }   

        private double modulusOfElasticity;
        public double ModulusOfElasticity
        {
            get { return modulusOfElasticity; }
            set
            {
                if (value != modulusOfElasticity)
                {
                    modulusOfElasticity = value;
                    RaisePropertyChanged(() => this.ModulusOfElasticity);
                }
            }
        }

        private double thermalCoefficient;
        public double ThermalCoefficient
        {
            get { return thermalCoefficient; }
            set
            {
                if (value != thermalCoefficient)
                {
                    thermalCoefficient = value;
                    RaisePropertyChanged(() => this.ThermalCoefficient);
                }
            }
        }
    }
}
