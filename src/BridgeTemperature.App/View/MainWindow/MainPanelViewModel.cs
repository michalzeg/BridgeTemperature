﻿using BridgeTemperature.Drawing;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.ObjectModel;
using BridgeTemperature.Calculations.Interfaces;
using BridgeTemperature.Calculations.Distributions;

namespace BridgeTemperature.ViewModel
{
    public class MainPanelViewModel : ViewModelBase
    {
        public IList<ISection> Sections { get; private set; }
        public bool ResultsUpToDate { get; set; }

        public MainPanelViewModel()
        {
            SectionDrawing = new List<SectionDrawingData>();
            ExternalDistributionDrawing = new List<DistributionDrawingData>();
            UniformDistributionDrawing = new List<DistributionDrawingData>();
            BendingDistributionDrawing = new List<DistributionDrawingData>();
            SelfEqulibratingDistributionDrawing = new List<DistributionDrawingData>();
            ResultsUpToDate = false;
            Sections = new List<ISection>();

            ExternalDistributionLabel = "External temperature";

            Messenger.Default.Register<ISection>(this, updateSections);
        }

        public IList<SectionDrawingData> SectionDrawing { get; set; }
        public IList<DistributionDrawingData> ExternalDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> UniformDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> BendingDistributionDrawing { get; set; }
        public IList<DistributionDrawingData> SelfEqulibratingDistributionDrawing { get; set; }

        public void ClearCanvas()
        {
            Sections.Clear();
            SectionDrawing.Clear();
            ClearDistributions();

            RaisePropertyChanged(() => SectionDrawing);
            RaisePropertyChanged(() => ExternalDistributionDrawing);
            RaisePropertyChanged(() => UniformDistributionDrawing);
            RaisePropertyChanged(() => BendingDistributionDrawing);
            RaisePropertyChanged(() => SelfEqulibratingDistributionDrawing);

            ExternalDistributionLabel = "";
            UniformDistributionLabel = "";
            BendingDistributionLabel = "";
            SelfDistributionLabel = "";
        }

        public void ClearDistributions()
        {
            ExternalDistributionDrawing.Clear();
            UniformDistributionDrawing.Clear();
            BendingDistributionDrawing.Clear();
            SelfEqulibratingDistributionDrawing.Clear();
        }

        public void ClearSections()
        {
            SectionDrawing.Clear();
        }

        public void UpdateDistribution(IEnumerable<Distribution> distribution, ISection section, Expression<Func<IList<DistributionDrawingData>>> property)
        {
            var expression = (MemberExpression)property.Body;
            var propertyInfo = (PropertyInfo)expression.Member;
            var currentPropertyValue = propertyInfo.GetValue(this) as IList<DistributionDrawingData>;
            var distributions = new List<DistributionDrawingData>(currentPropertyValue);
            distributions.Add(new DistributionDrawingData()
            {
                Distribution = distribution.ToList(),
                SectionMaxX = section.XMax,
                SectionMinX = section.XMin,
                SectionMaxY = section.YMax,
                SectionMinY = section.YMin,
            });
            propertyInfo.SetValue(this, distributions);
            RaisePropertyChanged(propertyInfo.Name);
        }

        private void updateSections(ISection section)
        {
            this.Sections.Add(section);
            ResultsUpToDate = false;
            var sections = SectionDrawing != null ? new List<SectionDrawingData>(SectionDrawing) : new List<SectionDrawingData>();
            sections.Add(new SectionDrawingData()
            {
                Coordinates = section.Coordinates,
                Type = section.Type
            });
            SectionDrawing = sections;
            UpdateDistribution(section.ExternalTemperature.Distribution, section, () => this.ExternalDistributionDrawing);
            RaisePropertyChanged(() => SectionDrawing);
        }

        private string externalDistributionLabel;

        public string ExternalDistributionLabel
        {
            get { return externalDistributionLabel; }
            set
            {
                if (value != externalDistributionLabel)
                {
                    externalDistributionLabel = value;
                    RaisePropertyChanged(() => ExternalDistributionLabel);
                }
            }
        }

        private string _uniformDistributionLabel;

        public string UniformDistributionLabel
        {
            get { return _uniformDistributionLabel; }
            set
            {
                if (value != _uniformDistributionLabel)
                {
                    _uniformDistributionLabel = value;
                    RaisePropertyChanged(() => UniformDistributionLabel);
                }
            }
        }

        private string _bendingDistributionLabel;

        public string BendingDistributionLabel
        {
            get { return _bendingDistributionLabel; }
            set
            {
                if (value != _bendingDistributionLabel)
                {
                    _bendingDistributionLabel = value;
                    RaisePropertyChanged(() => BendingDistributionLabel);
                }
            }
        }

        private string _selfDistributionLabel;

        public string SelfDistributionLabel
        {
            get { return _selfDistributionLabel; }
            set
            {
                if (value != _selfDistributionLabel)
                {
                    _selfDistributionLabel = value;
                    RaisePropertyChanged(() => SelfDistributionLabel);
                }
            }
        }
    }
}