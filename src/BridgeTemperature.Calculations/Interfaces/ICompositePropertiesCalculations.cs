namespace BridgeTemperature.Calculations.Interfaces
{
    public interface ICompositePropertiesCalculations : IIntegrable
    {
        double ModulusOfElasticity { get; }
        double Area { get; }
        double MomentOfInertia { get; }
    }
}