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
            Stress = new RelayCommand(this.updateStress);
            Temperature = new RelayCommand(this.updateTemperature);

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
        public RelayCommand Stress { get; private set; }
        public RelayCommand Temperature { get; private set; }


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



        private ICompositeSection compositeSection;
        private void run()
        {
            compositeSection = new CompositeSection(MainPanelVM.Sections);
            var calculator = new DistributionCalculations(compositeSection);
            calculator.CalculateDistributions();
            updateTemperature(); 
        }
        
        private void updateTemperature()
        {
            foreach (var section in compositeSection.Sections)
            {
                MainPanelVM.UpdateDistribution(section.ExternalTemperature.Distribution, section, () => MainPanelVM.ExternalDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.UniformTemperature.Distribution, section, () => MainPanelVM.UniformDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.BendingTemperature.Distribution, section, () => MainPanelVM.BendingDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.SelfEquilibratedTemperature.Distribution, section, () => MainPanelVM.SelfEqulibratingDistributionDrawing);
            }
            MainPanelVM.BendingDistributionLabel = "Bending Temperature";
            MainPanelVM.ExternalDistributionLabel = "External Temperature";
            MainPanelVM.UniformDistributionLabel = "Uniform Temperature";
            MainPanelVM.SelfDistributionLabel = "Selfequilibrating temperature";
        }
        private void updateStress()
        {
            foreach (var section in compositeSection.Sections)
            {
                MainPanelVM.UpdateDistribution(section.ExternalStress.Distribution, section, () => MainPanelVM.ExternalDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.UniformStress.Distribution, section, () => MainPanelVM.UniformDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.BendingStress.Distribution, section, () => MainPanelVM.BendingDistributionDrawing);
                MainPanelVM.UpdateDistribution(section.SelfEquilibratedStress.Distribution, section, () => MainPanelVM.SelfEqulibratingDistributionDrawing);
            }
            MainPanelVM.BendingDistributionLabel = "Bending Stress";
            MainPanelVM.ExternalDistributionLabel = "External Stress";
            MainPanelVM.UniformDistributionLabel = "Uniform Stress";
            MainPanelVM.SelfDistributionLabel = "Selfequilibrating Stress";
        }

    }
}