using BridgeTemperature.Drawing;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using BridgeTemperature.Sections;
using BridgeTemperature.DistributionOperations;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;

namespace BridgeTemperature.ViewModel
{
    public class MainPanelViewModel :ViewModelBase
    {
        public IList<ISection> Sections { get; private set; }
        public bool ResultsActual { get; set; }

        public MainPanelViewModel()
        {
            SectionDrawing = new List<SectionDrawingData>();
            ExternalDistributionDrawing = new List<DistributionDrawingData>();
            UniformDistributionDrawing = new List<DistributionDrawingData>();
            BendingDistributionDrawing = new List<DistributionDrawingData>();
            SelfEqulibratingDistributionDrawing = new List<DistributionDrawingData>();
            ResultsActual = false;
            Sections = new List<ISection>();

            ExternalDistributionLabel = "External temperature";

            Messenger.Default.Register<ISection>(this, updateSections);
            
        }

        public IList<SectionDrawingData> SectionDrawing { get; set; }
        public IList<DistributionDrawingData> ExternalDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> UniformDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> BendingDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> SelfEqulibratingDistributionDrawing { get; set; }


        public void ClearCanvas()
        {
            Sections.Clear();
            SectionDrawing.Clear(); //= null;
            ClearDistributions();

            RaisePropertyChanged(() => SectionDrawing);
            RaisePropertyChanged(() => ExternalDistributionDrawing);
            RaisePropertyChanged(() => UniformDistributionDrawing);
            RaisePropertyChanged(() => BendingDistributionDrawing);
            RaisePropertyChanged(() => SelfEqulibratingDistributionDrawing);

            ExternalDistributionLabel = "";
            UniformDistributionLabel = "";
            BendingDistributionLabel = "";
            SelfDistributionLabel = "";
        }

        public void ClearDistributions()
        {
            //SectionDrawing.Clear();
            ExternalDistributionDrawing.Clear();
            UniformDistributionDrawing.Clear();
            BendingDistributionDrawing.Clear();
            SelfEqulibratingDistributionDrawing.Clear();
        }
        public void ClearSections()
        {
            SectionDrawing.Clear();
        }

        public void UpdateDistribution(IEnumerable<Distribution> distribution,ISection section, Expression<Func<IList<DistributionDrawingData>>> property)
        {
            
            var expression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var currentPropertyValue = propertyInfo.GetValue(this) as IList<DistributionDrawingData>;
            var distributions = new List<DistributionDrawingData>(currentPropertyValue);
            distributions.Add(new DistributionDrawingData()
            {
                Distribution = distribution.ToList(),
                SectionMaxX = section.XMax,
                SectionMinX = section.XMin,
                SectionMaxY = section.YMax,
                SectionMinY = section.YMin,
            });
            propertyInfo.SetValue(this, distributions);
            RaisePropertyChanged(propertyInfo.Name);
    
        }
        private void updateSections(ISection section)
        {
            this.Sections.Add(section);
            ResultsActual = false;
            var sections = SectionDrawing != null ? new List<SectionDrawingData>(SectionDrawing) : new List<SectionDrawingData>();
            sections.Add(new SectionDrawingData()
            {
                Coordinates = section.Coordinates,
                Type = section.Type
            });
            SectionDrawing = sections;

            
            UpdateDistribution(section.ExternalTemperature.Distribution,section, () => this.ExternalDistributionDrawing);


            RaisePropertyChanged(() => SectionDrawing);
        }

        private string externalDistributionLabel;
        public string ExternalDistributionLabel
        {
            get { return externalDistributionLabel; }
            set
            {
                if (value != externalDistributionLabel)
                {
                    externalDistributionLabel = value;
                    RaisePropertyChanged(() => ExternalDistributionLabel);
                }
            }
        }
        private string uniformDistributionLabel;
        public string UniformDistributionLabel
        {
            get { return uniformDistributionLabel; }
            set
            {
                if (value != uniformDistributionLabel)
                {
                    uniformDistributionLabel = value;
                    RaisePropertyChanged(() => UniformDistributionLabel);
                }
            }
        }
        private string bendingDistributionLabel;
        public string BendingDistributionLabel
        {
            get { return bendingDistributionLabel; }
            set
            {
                if (value != bendingDistributionLabel)
                {
                    bendingDistributionLabel = value;
                    RaisePropertyChanged(() => BendingDistributionLabel);
                }
            }
        }
        private string selfDistributionLabel;
        public string SelfDistributionLabel
        {
            get { return selfDistributionLabel; }
            set
            {
                if (value != selfDistributionLabel)
                {
                    selfDistributionLabel = value;
                    RaisePropertyChanged(() => SelfDistributionLabel);
                }
            }
        }
    }
}
