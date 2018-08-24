using BridgeTemperature.IntegrationFunctions;

namespace BridgeTemperature.SectionProperties
{
    public interface ICompositePropertiesCalculations : IIntegrable
    {
        double ModulusOfElasticity { get; }
        double Area { get; }
        double MomentOfInertia { get; }
    }
}