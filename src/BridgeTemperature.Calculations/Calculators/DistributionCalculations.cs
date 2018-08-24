using System;
using System.Linq;
using System.Collections.Generic;
using BridgeTemperature.Sections;
using BridgeTemperature.IntegrationFunctions;

namespace BridgeTemperature.DistributionOperations
{
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

        private ICompositeSection compositeSection;

        public DistributionCalculations(ICompositeSection compositeSection)
        {
            this.compositeSection = compositeSection;
        }

        private double CalculateNormalForce()
        {
            var integration = new Integration();
            foreach (var section in compositeSection.Sections)
            {
                integration.Integrate(section, compositeSection.CentreOfGravity, section.ExternalStress.GetValue);
            }
            return integration.NormalForce;
        }

        private void CalculateNormalStress(double normalForce)
        {
            foreach (var section in compositeSection.Sections)
            {
                var uniformDistribution = StressDistribution.AxialStress(section.Coordinates, normalForce, compositeSection.Area, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.UniformStress = uniformDistribution;
                section.UniformStress.MultiplyDistribution(-1);
            }
        }

        private double CalculateBendingMoment()
        {
            var integration = new Integration();
            foreach (var section in compositeSection.Sections)
            {
                var uniformPlusSelfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                uniformPlusSelfEquilibratingStress.SubtractDistribution(section.UniformStress.Distribution);
                integration.Integrate(section, compositeSection.CentreOfGravity, uniformPlusSelfEquilibratingStress.GetValue);
            }
            return integration.Moment;
        }

        private void CalculateBendingAndSelfStresses(double moment)
        {
            foreach (var section in compositeSection.Sections)
            {
                StressDistribution bendingDistribution = StressDistribution.BendingStress(section.Coordinates, moment, compositeSection.CentreOfGravity.Y, compositeSection.MomentOfIntertia, compositeSection.BaseModulusOfElasticity, section.ModulusOfElasticity);
                section.BendingStress = bendingDistribution;
                section.BendingStress.MultiplyDistribution(-1);

                var selfEquilibratingStress = new StressDistribution(section.ExternalStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.UniformStress.Distribution);
                selfEquilibratingStress.AddDistribution(section.BendingStress.Distribution);
                section.SelfEquilibratedStress = selfEquilibratingStress;
                section.SelfEquilibratedStress.MultiplyDistribution(-1);
            }
        }

        private void ConvertStressToTemperature()
        {
            foreach (var section in compositeSection.Sections)
            {
                section.UniformTemperature = section.UniformStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.BendingTemperature = section.BendingStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
                section.SelfEquilibratedTemperature = section.SelfEquilibratedStress.ConvertToTemperatureDistribution(section.Coordinates, section.ModulusOfElasticity, section.ThermalCooeficient);
            }
        }

        public void CalculateDistributions()
        {
            var normalForce = CalculateNormalForce();
            CalculateNormalStress(normalForce);
            var moment = CalculateBendingMoment();
            CalculateBendingAndSelfStresses(moment);
            ConvertStressToTemperature();
        }

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