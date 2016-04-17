/*
 * Created by SharpDevelop.
 * User: MZ036205
 * Date: 31/12/2015
 * Time: 11:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BridgeTemperature.Helpers;
using BridgeTemperature.Sections;
using BridgeTemperature.Extensions;
using BridgeTemperature.IntegrationFunctions;
using BridgeTemperature.Common;

namespace BridgeTemperature.DistributionOperations
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

    public enum ResultType
    {
        ExternalStress,
        BendingStress,
        UniformStress,
        SelfEquilibratedStress,

        ExternalTemperature,
        BendingTemperature,
        UniformTemperature,
        SelfEquilibratedTemperature,
    }

    public class Distribution : IEquatable<Distribution>
    {
        public double Y { get; set; }
        public double Value { get; set; }

        public Distribution()
        { }
        public Distribution(double y, double value)
        {
            Y = y;
            Value = value;
        }

        public PointD ConvertToPointD()
        {
            return new PointD(Value, Y);
        }

        public bool Equals(Distribution other)
        {
            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, other)) return true;


            //Check whether the products' properties are equal. 
            return Y.IsApproximatelyEqualTo(other.Y) && Value.IsApproximatelyEqualTo(other.Value);
        }
        public override int GetHashCode()
        {
            //Get hash code for the Name field if it is not null. 
            int hashY = Y.GetHashCode();

            //Get hash code for the Code field. 
            int hashValue = Value.GetHashCode();

            //Calculate the hash code for the product. 
            return hashY ^ hashValue;
        }


    }



    public abstract class BaseDistribution
    {
        private enum operationType { Addition, Subtraction };


        protected Interpolation interpolation;
        public IEnumerable<Distribution> Distribution { get; private set; }

        protected BaseDistribution(IEnumerable<Distribution> distribution)
        {
            this.updateDistribution(distribution);
        }

        private void updateDistribution(IEnumerable<Distribution> distribution)
        {
            var x = distribution.Select(e => e.Y);
            var y = distribution.Select(e => e.Value);
            this.interpolation = new Interpolation(x, y);
            this.Distribution = distribution;
        }

        public virtual double GetValue(double y)
        {

            return this.interpolation.Interpolate(y);
        }

        public void AddDistribution(IEnumerable<Distribution> distribution)
        {
            this.addOrSubtract(distribution, operationType.Addition);
        }
        public void SubtractDistribution(IEnumerable<Distribution> distribution)
        {
            this.addOrSubtract(distribution, operationType.Subtraction);
        }
        public void MultiplyDistribution(double value)
        {
            var multipliedDistribution = new List<Distribution>();
            foreach (var element in this.Distribution)
            {
                Distribution distribution = new Distribution();
                distribution.Value = element.Value * value;
                distribution.Y = element.Y;
                multipliedDistribution.Add(distribution);
            }
            updateDistribution(multipliedDistribution.Distinct().OrderBy(e => e.Y));
        }
        private void addOrSubtract(IEnumerable<Distribution> distribution, operationType operationType)
        {
            var distributionSum = new List<Distribution>();
            var tempInterpolation = new Interpolation(distribution.Select(e => e.Y), distribution.Select(e => e.Value));
            int multiplier = (operationType == operationType.Addition) ? 1 : -1;
            foreach (var element in this.Distribution)
            {
                var newDistribution = new Distribution();
                newDistribution.Y = element.Y;
                newDistribution.Value = element.Value + tempInterpolation.Interpolate(element.Y) * multiplier;
                distributionSum.Add(newDistribution);
            }

            foreach (var element in distribution)
            {
                var newDistribution = new Distribution();
                newDistribution.Y = element.Y;
                newDistribution.Value = this.interpolation.Interpolate(element.Y) + element.Value * multiplier;
                distributionSum.Add(newDistribution);
            }

            updateDistribution(distributionSum.Distinct().OrderBy(e => e.Y));

        }
    }

    public class TemperatureDistribution : BaseDistribution
    {
        public TemperatureDistribution(IEnumerable<Distribution> distribution) : base(distribution)
        {

        }

        public StressDistribution ConvertToStressDistribution(IEnumerable<PointD> coordinates, double modulusOfElasticity, double thermalCoefficient)
        {
            var stressDistribution = new List<Distribution>();
            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = this.interpolation.Interpolate(coordinate.Y) * thermalCoefficient * modulusOfElasticity;
                stressDistribution.Add(distribution);
            }
            foreach (var baseDistribution in base.Distribution)
            {
                var distribution = new Distribution();
                distribution.Y = baseDistribution.Y;
                distribution.Value = baseDistribution.Value * thermalCoefficient * modulusOfElasticity;
                stressDistribution.Add(distribution);
            }

            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }
    }

    public class StressDistribution : BaseDistribution
    {
        public StressDistribution(IEnumerable<Distribution> distribution) : base(distribution)
        {

        }
        public TemperatureDistribution ConvertToTemperatureDistribution(IEnumerable<PointD> coordinates, double modulusOfElasticity, double thermalCoefficient)
        {
            var temperatureDistribution = new List<Distribution>();

            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = base.GetValue(coordinate.Y) / thermalCoefficient / modulusOfElasticity;
                temperatureDistribution.Add(distribution);
            }
            foreach (var baseDistribution in base.Distribution)
            {
                var distribution = new Distribution();
                distribution.Y = baseDistribution.Y;
                distribution.Value = baseDistribution.Value / thermalCoefficient / modulusOfElasticity;
                temperatureDistribution.Add(distribution);
            }
            return new TemperatureDistribution(temperatureDistribution.Distinct().OrderBy(e => e.Y));
        }


        public static StressDistribution BendingStress(IEnumerable<PointD> coordinates, double bendingMoment, double centreOfGravity, double momentOfInertia, double baseModulusOfElasticity, double modulusOfElasticity)
        {
            var stressDistribution = new List<Distribution>();


            foreach (var coordinate in coordinates)
            {
                double alfa = baseModulusOfElasticity / modulusOfElasticity;
                var distribution = new Distribution();
                distribution.Y = coordinate.Y;
                distribution.Value = bendingMoment / momentOfInertia * (centreOfGravity - coordinate.Y) / alfa;
                stressDistribution.Add(distribution);
            }
            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }
        public static StressDistribution AxialStress(IEnumerable<PointD> coordinates, double axialForce, double area, double baseModulusOfElasticity, double modulusOfElasticity)
        {
            var stressDistribution = new List<Distribution>();

            foreach (var coordinate in coordinates)
            {
                var distribution = new Distribution();
                var alfa = baseModulusOfElasticity / modulusOfElasticity;
                distribution.Y = coordinate.Y;
                distribution.Value = axialForce / area / alfa;
                stressDistribution.Add(distribution);
            }

            return new StressDistribution(stressDistribution.Distinct().OrderBy(e => e.Y));
        }

    }

    public class DistributionCalculations
    {
        //algorithm
        /*1) externam temperature and external stress
        2) interigate external stress and obtain uniformy distributed stress and temperature
        3) subtract uniform stress from external stress and interigate it to obtain bending stress
        4) subtrack uniform and bending stresses from externat in order to obtain selfequilibrating stresses
        5) convert all stresses to temperature*/


        //public IEnumerable<IEnumerable<Distribution>> StressResults { get; private set; }
        //public IEnumerable<IEnumerable<Distribution>> TemperatureResults { get; private set; }

        ICompositeSection compositeSection;

        public DistributionCalculations(ICompositeSection compositeSection)
        {
            this.compositeSection = compositeSection;
            
        }

        public void CalculateDistributions()
        {
            Integration integration = new Integration();

            foreach(var section in compositeSection.Sections)
            {
                integration.Integrate(section, section.ExternalStress.GetValue);
            }
            foreach (var section in compositeSection.Sections)
            {
                StressDistribution uniformDistribution = StressDistribution.AxialStress(section.Coordinates, integration.NormalForce, compositeSection.Area, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.UniformStress = uniformDistribution;
                section.UniformStress.MultiplyDistribution(-1);   
            }

            integration.Reset();
            foreach (var section in compositeSection.Sections)
            {
                var uniformPlusSelfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                uniformPlusSelfEquilibratingStress.SubtractDistribution(section.UniformStress.Distribution);
                integration.Integrate(section, uniformPlusSelfEquilibratingStress.GetValue);
            }

            foreach (var section in compositeSection.Sections)
            {
                
                StressDistribution bendingDistribution = StressDistribution.BendingStress(section.Coordinates, integration.Moment, compositeSection.CentreOfGravity.Y, compositeSection.MomentOfIntertia, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.BendingStress = bendingDistribution;
                section.BendingStress.MultiplyDistribution(-1);

                var selfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.UniformStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.BendingStress.Distribution);
                section.SelfEquilibratedStress = selfEquilibratingStress;
                section.SelfEquilibratedStress.MultiplyDistribution(-1);

                //converting to Temperature
                section.UniformTemperature = section.UniformStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.BendingTemperature = section.BendingStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.SelfEquilibratedTemperature = section.SelfEquilibratedStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
            }
        }
        /*public void CalculateDistributions()
        {

            foreach (var section in compositeSection.Sections)
            {
                Integration integration = new Integration();
                integration.Integrate(section, section.ExternalStress.GetValue);
                StressDistribution uniformDistribution = StressDistribution.AxialStress(section.Coordinates, integration.NormalForce, compositeSection.Area, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.UniformStress = uniformDistribution;
                section.UniformStress.MultiplyDistribution(-1);

                var uniformPlusSelfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                uniformPlusSelfEquilibratingStress.SubtractDistribution(section.UniformStress.Distribution);
                integration.Integrate(section, uniformPlusSelfEquilibratingStress.GetValue);
                StressDistribution bendingDistribution = StressDistribution.BendingStress(section.Coordinates, integration.Moment, compositeSection.CentreOfGravity.Y, compositeSection.MomentOfIntertia, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.BendingStress = bendingDistribution;
                section.BendingStress.MultiplyDistribution(-1);

                var selfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.UniformStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.BendingStress.Distribution);
                section.SelfEquilibratedStress = selfEquilibratingStress;
                section.SelfEquilibratedStress.MultiplyDistribution(-1);

                //converting to Temperature
                section.UniformTemperature = section.UniformStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.BendingTemperature = section.BendingStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.SelfEquilibratedTemperature = section.SelfEquilibratedStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
            }
        }*/

        public IEnumerable<IEnumerable<Distribution>> GetResult(ResultType resultType)
        {

            var resultList = new List<IEnumerable<Distribution>>();

            foreach (var section in this.compositeSection.Sections)
            {
                var propertyName = resultType.ToString();
                var propertyInfo = section.GetType().GetProperty(propertyName);
                if (propertyInfo == null)
                    throw new NullReferenceException();
                var value = propertyInfo.GetValue(section, null) as BaseDistribution;
                resultList.Add(value.Distribution.ToList());
            }
            return resultList;
        }


    }


}
