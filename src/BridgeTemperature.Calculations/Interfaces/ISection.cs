using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using BridgeTemperature.SectionProperties;
using BridgeTemperature.IntegrationFunctions;
using BridgeTemperature.DistributionOperations;

namespace BridgeTemperature.Sections
{
    public interface ISection : ICompositePropertiesCalculations, IDistributable
    {
        double ThermalCooeficient { get; }
        double XMax { get; }
        double XMin { get; }
        double Height { get; }
    }
}