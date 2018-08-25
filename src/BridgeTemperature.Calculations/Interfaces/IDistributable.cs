using BridgeTemperature.Calculations.Distributions;

namespace BridgeTemperature.Calculations.Interfaces
{
    public interface IDistributable
    {
        StressDistribution ExternalStress { get; }
        StressDistribution BendingStress { get; set; }
        StressDistribution UniformStress { get; set; }
        StressDistribution SelfEquilibratedStress { get; set; }

        TemperatureDistribution ExternalTemperature { get; }
        TemperatureDistribution BendingTemperature { get; set; }
        TemperatureDistribution UniformTemperature { get; set; }
        TemperatureDistribution SelfEquilibratedTemperature { get; set; }
    }
}