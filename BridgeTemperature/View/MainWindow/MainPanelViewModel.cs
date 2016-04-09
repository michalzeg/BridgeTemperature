using BridgeTemperature.Drawing;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using BridgeTemperature.Sections;

namespace BridgeTemperature.ViewModel
{
    public class MainPanelViewModel :ViewModelBase
    {
        public MainPanelViewModel()
        {
            Sections = new List<SectionDrawingData>();
            ExternalDistribution = new List<DistributionDrawingData>();

            Messenger.Default.Register<ISection>(this, updateSections);
            Messenger.Default.Register<MessengerTokens>(this, MessengerTokens.ClearDrawings, clearDrawings);
        }

        public IList<SectionDrawingData> Sections { get; set; }
        public IList<DistributionDrawingData> ExternalDistribution { get; set; }
        public IList<DistributionDrawingData> UniformDistribution { get; set; }
        public IList<DistributionDrawingData> BendingDistribution { get; set; }
        public IList<DistributionDrawingData> SelfEqulibratingDistribution { get; set; }


        private void clearDrawings(MessengerTokens token)
        {

        }

        private void updateDistribution()
        {
            
        }
        private void updateSections(ISection section)
        {
            var sections = new List<SectionDrawingData>(Sections);
            sections.Add(new SectionDrawingData()
            {
                Coordinates = section.Coordinates,
                Type = section.Type
            });
            Sections = sections;

            var distributions = new List<DistributionDrawingData>(ExternalDistribution);
            distributions.Add(new DistributionDrawingData()
            {
                Distribution = section.ExternalTemperature.Distribution.ToList(),
                SectionMaxX = section.XMax,
                SectionMinX = section.XMin,
                SectionMaxY = section.YMax,
                SectionMinY = section.YMin,
            });
            ExternalDistribution = distributions;

            RaisePropertyChanged(() => Sections);
            RaisePropertyChanged(() => ExternalDistribution);
        }

    }
}
