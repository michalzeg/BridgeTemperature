using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BridgeTemperature.MaterialProperties
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

    public class MaterialProvider
    {
        public static IEnumerable<Material> GetAllMaterials()
        {
            IEnumerable<Material> materials;

            var location = Path.GetDirectoryName(typeof(Material).Assembly.Location);

            var filePath = Path.Combine(location, "Resources", "materials.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(List<Material>));
            using (var reader = new StreamReader(filePath))
            {
                materials = serializer.Deserialize(reader) as IEnumerable<Material>;
            }
            return materials;
        }

        public static IEnumerable<Material> GetSteelMaterials()
        {
            var materials = GetAllMaterials();

            return materials.Where(e => e.Grade[0] == 'S');
        }

        public static IEnumerable<Material> GetConcreteMaterials()
        {
            var materials = GetAllMaterials();
            return materials.Where(e => e.Grade[0] == 'C');
        }
    }
}