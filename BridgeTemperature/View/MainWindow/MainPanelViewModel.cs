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


            Messenger.Default.Register<ISection>(this, updateSections);
            Messenger.Default.Register<MessengerTokens>(this, MessengerTokens.ClearDrawings, clearDrawings);
        }

        public IList<SectionDrawingData> Sections { get; set; }
        public DistributionDrawingData ExternalDistribution { get; set; }
        public DistributionDrawingData UniformDistribution { get; set; }
        public DistributionDrawingData BendingDistribution { get; set; }
        public DistributionDrawingData SelfEqulibratingDistribution { get; set; }


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
            RaisePropertyChanged(() => Sections);
        }

    }
}
