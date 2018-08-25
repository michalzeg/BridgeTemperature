using System;
using System.Text;
using System.Xml.Serialization;

namespace BridgeTemperature.Common.Materials
{
    public class Material
    {
        [XmlElement()]
        public string Type { get; set; }

        [XmlElement()]
        public string Grade { get; set; }

        [XmlElement()]
        public double E { get; set; }

        [XmlElement()]
        public double ThermalCoefficient { get; set; }
    }
}