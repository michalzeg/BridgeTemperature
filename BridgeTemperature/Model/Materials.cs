using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BridgeTemperature.Materials
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
        public string Grade { get; set; } //nazwa klasy betonu


        [XmlElement()]
        public double E { get; set; }

        [XmlElement()]
        public double ThermalCoefficient { get; set; }
    }

    /*public class Concrete :IMaterial
    {
        //opisuje właściwosci betonu
        [XmlElement()]
        public string Grade { get; set; } //nazwa klasy betonu

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
        public static IEnumerable<Material> GetMaterials()
        {
            IEnumerable<Material> materials;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Material>));


            using (var reader = new StringReader(Properties.Resources.materials)) 
            {
                materials = serializer.Deserialize(reader) as IEnumerable<Material>;
            }
            return materials;
        }
        public static IEnumerable<string> GetMaterialNames()
        {

            var materials = GetMaterials();

            if (materials == null)
                throw new NullReferenceException("xml file with materials is empty");

            var namesOfMaterials = materials.Select(e => e.Grade);
            return namesOfMaterials;
        }
        public static void GetMaterialProperties(string materialName, out double modulusOfElasticity, out double thermalCoefficient)
        {
            var materials = GetMaterials();
            var namesOfMaterials = GetMaterialNames();
            if (!materials.Any(e => e.Grade == materialName))
                throw new NullReferenceException(string.Format("Xml file does not contain {0} name", materialName));

            thermalCoefficient = materials.First(e => e.Grade == materialName).ThermalCoefficient;
            modulusOfElasticity = materials.First(e => e.Grade == materialName).E;
        }

    }



    
}
