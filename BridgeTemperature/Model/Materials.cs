using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BridgeTemperature.MaterialProperties
{
    /*public class MaterialList
    {
        [XmlElement("Concrete")]
        public List<Concrete> ConcreteList { get; set; }
        [XmlElement("StructuralSteel")]
        public List<StructuralSteel> StructuralSteelList { get; set; }
        
    }*/
    
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

    /*public class Concrete :IMaterial
    {
        
        [XmlElement()]
        public string Grade { get; set; } 

        [XmlElement()]
        public double E {get;set;}
        
        [XmlElement()]
        public double ThermalCoefficient {get;set;}
    }

    public class StructuralSteel :IMaterial
    {
        [XmlElement()]
        public string Grade { get; set; }

        [XmlElement()]
        public double E { get; set; }
        
        [XmlElement()]
        public double ThermalCoefficient {get;set;}
    }*/

    public class MaterialOperations
    {
        public static IEnumerable<Material> GetAllMaterials()
        {
            IEnumerable<Material> materials;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Material>));
            using (var reader = new StringReader(Properties.Resources.materials)) 
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
