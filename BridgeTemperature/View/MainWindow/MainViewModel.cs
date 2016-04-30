using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using BridgeTemperature.Common;
using BridgeTemperature.View;
using BridgeTemperature.Sections;
using BridgeTemperature.DistributionOperations;
using Xceed.Wpf.Toolkit;
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
            New = new RelayCommand(@new);
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
        public RelayCommand New { get; private set; }
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

        private void @new()
        {
            MainPanelVM.ClearCanvas();
        }

        private void openPlateGirderWindow()
        {
            var steelWindow = new SteelWindow();
            var vm = new SteelWindowViewModel();
            steelWindow.DataContext = vm;
            //vm.updateDrawings();
            steelWindow.Show();
        }
        private void openCompositeGirderSimplifiedWindow()
        {
            var composteWindow = new SimplifiedCompositeWindow();
            var vm = new SimplifiedCompositeWindowViewModel();
            composteWindow.DataContext = vm;
            composteWindow.Show();
        }
        private void openCompositeGirderNormalWindow()
        {
            var composteWindow = new NormalCompositeWindow();
            var vm = new NormalCompositeWindowViewModel();
            composteWindow.DataContext = vm;
            composteWindow.Show();
        }
        private void openSlabWindow() {  }
        private void openIGirderWindow()
        {
            var cconcreteIGirderWindow = new ConcreteIGirderWindow();
            var vm = new ConcreteIGriderWindowViewModel();
            cconcreteIGirderWindow.DataContext = vm;
            cconcreteIGirderWindow.Show();
        }
        private void openBoxGirderWindow()
        {
            var boxGirderWindow = new BoxGirderWindow();
            var vm = new BoxGriderWindowViewModel();
            boxGirderWindow.DataContext = vm;
            boxGirderWindow.Show();
        }
        private void openRectangularSectionWindow()
        {
            var rectangularWindow = new RectangularWindow();
            var vm = new RectangularWindowViewModel();
            rectangularWindow.DataContext = vm;
            rectangularWindow.Show();
        }
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
            if (MainPanelVM.Sections == null || MainPanelVM.Sections.Count == 0)
                MessageBox.Show("Wrong section data", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
            {
                compositeSection = new CompositeSection(MainPanelVM.Sections);
                var calculator = new DistributionCalculations(compositeSection);
                calculator.CalculateDistributions();
                MainPanelVM.ResultsActual = true;
                updateTemperature();
                
            }
        }
        
        private void updateTemperature()
        {
            if (!MainPanelVM.ResultsActual)
                MessageBox.Show("No data", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
            {
                MainPanelVM.ClearDistributions();
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
        }
        private void updateStress()
        {
            if (!MainPanelVM.ResultsActual)
                MessageBox.Show("No data", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            else
            {
                MainPanelVM.ClearDistributions();
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
}