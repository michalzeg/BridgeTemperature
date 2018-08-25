using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace BridgeTemperature.Calculations.Interfaces
{
    public interface ISection : ICompositePropertiesCalculations, IDistributable
    {
        double ThermalCooeficient { get; }
        double XMax { get; }
        double XMin { get; }
        double Height { get; }
    }
}