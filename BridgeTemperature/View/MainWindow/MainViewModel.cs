using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using BridgeTemperature.Common;
using BridgeTemperature.Sections;
using BridgeTemperature.DistributionOperations;
namespace BridgeTemperature.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainPanelViewModel MainPanelVM { get; private set; }

        public MainViewModel()
        {
            OpenPlateGirderWindow = new RelayCommand(this.openPlateGirderWindow);
            OpenCompositeGirderSimplifiedWindow = new RelayCommand(this.openCompositeGirderSimplifiedWindow);
            OpenCompositeGirderNormalWindow = new RelayCommand(this.openCompositeGirderNormalWindow);
            OpenSlabWindow = new RelayCommand(this.openSlabWindow);
            OpenIGirderWindow = new RelayCommand(this.openIGirderWindow);
            OpenBoxGirderWindow = new RelayCommand(this.openBoxGirderWindow);
            OpenRectangularSectionWindow = new RelayCommand(this.openRectangularSectionWindow);
            OpenCustomWindow = new RelayCommand(this.openCustomWindow);

            Run = new RelayCommand(this.run);


            MainPanelVM = new MainPanelViewModel();
            
        }

        public RelayCommand OpenPlateGirderWindow { get; private set; }
        public RelayCommand OpenCompositeGirderSimplifiedWindow { get; private set; }
        public RelayCommand OpenCompositeGirderNormalWindow { get; private set; }
        public RelayCommand OpenSlabWindow { get; private set; }
        public RelayCommand OpenIGirderWindow{get;private set;}
        public RelayCommand OpenBoxGirderWindow { get; private set; }
        public RelayCommand OpenRectangularSectionWindow { get; private set; }
        public RelayCommand OpenCustomWindow { get; private set; }
        public RelayCommand Run { get; private set; }


        private void openPlateGirderWindow()
        {
            var steelWindow = new SteelWindow();
            var vm = new SteelWindowViewModel();
            steelWindow.DataContext = vm;
            //vm.updateDrawings();
            steelWindow.Show();
        }
        private void openCompositeGirderSimplifiedWindow() {  }
        private void openCompositeGirderNormalWindow() {  }
        private void openSlabWindow() {  }
        private void openIGirderWindow() {  }
        private void openBoxGirderWindow() {  }
        private void openRectangularSectionWindow() {  }
        private void openCustomWindow()
        {
            var customWindow = new CustomWindow();
            var vm = new CustomWindowViewModel();
            customWindow.DataContext = vm;
            customWindow.Show();
        }

        private void run()
        {
            var compositeSection = new CompositeSection(MainPanelVM.Sections);
            var calculator = new DistributionCalculations(compositeSection);
            calculator.CalculateDistributions();

            foreach (var section in compositeSection.Sections)
            {
                MainPanelVM.UpdateDistribution(section.ExternalTemperature.Distribution, section, () => MainPanelVM.ExternalDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.UniformTemperature.Distribution, section, () => MainPanelVM.UniformDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.BendingTemperature.Distribution, section, () => MainPanelVM.BendingDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.SelfEquilibratedTemperature.Distribution, section, () => MainPanelVM.SelfEqulibratingDistributionDrawing);
            }
        }
        

    }
}