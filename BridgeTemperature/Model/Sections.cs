using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using BridgeTemperature.Extensions;
using BridgeTemperature.SectionProperties;
using BridgeTemperature.Helpers;
using BridgeTemperature.IntegrationFunctions;
using BridgeTemperature.DistributionOperations;
namespace BridgeTemperature.Sections
{
    
    
    public interface ICompositeSection
    {
        ICollection<ISection> Sections { get; }
        PointD CentreOfGravity { get; }
        double MomentOfIntertia { get; }
        double BaseModulusOfElasticity { get; }
        double Area { get; }
    }

    public class CompositeSection : ICompositeSection
    {
        public double BaseModulusOfElasticity { get; private set; }
        public PointD CentreOfGravity { get; private set; }
        public double MomentOfIntertia { get; private set; }
        public ICollection<ISection> Sections { get; private set; }
        public double Area {get; private set;}

        public CompositeSection(ICollection<ISection> sections)
        {
            var compositeProperties = new CompositeSectionPropertiesCalculations(sections);
            this.Sections = sections;
            this.BaseModulusOfElasticity = compositeProperties.BaseModulusOfElasticity;
            this.CentreOfGravity = compositeProperties.CentreOfGravity;
            this.Area = compositeProperties.Area;
            this.MomentOfIntertia = compositeProperties.SecondMomentOfArea;
        }


    }


    public interface ISection : ICompositePropertiesCalculations, IDistributable
    {
        double ThermalCooeficient { get; }
        double XMax { get; }
        double XMin { get; }
        double Height { get; }

        //IList<PointD> Coordinates { get; }
    }

    public class Section : ISection
    {
        public StressDistribution ExternalStress { get; private set; }
        public StressDistribution BendingStress { get; set; }
        public StressDistribution UniformStress { get; set; }
        public StressDistribution SelfEquilibratedStress { get; set; }

        public TemperatureDistribution ExternalTemperature { get; private set; }
        public TemperatureDistribution BendingTemperature { get; set; }
        public TemperatureDistribution UniformTemperature { get; set; }
        public TemperatureDistribution SelfEquilibratedTemperature { get; set; }

        public SectionType Type { get; }
        public double ModulusOfElasticity { get; }
        public double ThermalCooeficient { get; }
        public double MomentOfInertia { get; }
        public double Area { get; }
        public PointD CentreOfGravity { get; }
        public double YMax { get; }
        public double YMin { get; }
        public double XMax { get; }
        public double XMin { get; }
        public double Height { get;}
        public IList<PointD> Coordinates { get; }

        public Section(IList<PointD> coordinates, SectionType type, double modulusOfElasticity, double thermalCooefficient,IEnumerable<Distribution> externalTemperatureDistribution)
        {
            this.Coordinates = checkIfCoordinatesAreClockwise(coordinates);
            this.checkLastElement();
            this.Type = type;
            this.ModulusOfElasticity = modulusOfElasticity;
            this.ThermalCooeficient = thermalCooefficient;

            SectionPropertiesCalculations properties = new SectionPropertiesCalculations(this.Coordinates);

            this.Area = properties.A;
            this.CentreOfGravity = properties.CentreOfGravity;
            this.MomentOfInertia = properties.Ix0;
            this.YMax = properties.YMax;
            this.YMin = properties.YMin;
            this.XMax = properties.XMax;
            this.XMin = properties.XMin;
            this.Height = properties.YMax - properties.YMin;

            this.ExternalTemperature = new TemperatureDistribution(externalTemperatureDistribution.OrderBy(e=>e.Y));
            this.ExternalStress = this.ExternalTemperature.ConvertToStressDistribution(coordinates, modulusOfElasticity, thermalCooefficient);
        }
        private void checkLastElement()
        {
            //checks if last element is equal to first
            PointD firstPoint = this.Coordinates[0];
            PointD lastPoint = this.Coordinates.Last();

            if (firstPoint.X.IsApproximatelyEqualTo(lastPoint.X) && firstPoint.Y.IsApproximatelyEqualTo(lastPoint.Y)) //POPRAWIC POROWNYWANIE
            {
                return;
            }
            else
            {
                this.Coordinates.Add(firstPoint);
            }
        }
        private IList<PointD> checkIfCoordinatesAreClockwise(IList<PointD> coordinates) //procedura sprawdza czy wspolrzedne przekroju sa wprowadzone zgodnie ze wskazowkami zegara
        {
            //function checks if coordinates are in clockwise or counterclockwise order. To check that cross product is used.
            //
            if (coordinates.Count < 3)
                throw new ArgumentOutOfRangeException();
            double crossPrd;
            List<PointD> tempCoord = new List<PointD>(coordinates);
            for (int i = 0; i <= coordinates.Count - 3; i++)
            {
                crossPrd = this.crossProduct(coordinates[i], coordinates[i + 1], coordinates[i + 2]);
                if (crossPrd > 0)
                {
                    //clockwise
                    break;
                }
                else if (crossPrd < 0)
                {
                    //counterclockwise

                    tempCoord.Reverse();
                    break;
                }
                else
                {
                    //parallel vectors, take next two points
                }
            }
            return tempCoord;
        }

        private double crossProduct(PointD p0, PointD p1, PointD p2)
        {
            double[] vector1 = new double[2];
            double[] vector2 = new double[2];

            vector1[0] = p1.X - p0.X;
            vector1[1] = p1.Y - p0.Y;
            vector2[0] = p2.X - p1.X;
            vector2[1] = p2.Y - p1.Y;

            double result; //ax*by-ay*bz
            result = vector1[0] * vector2[1] - vector1[1] * vector2[0];
            return result;
        }
    }

    

}
