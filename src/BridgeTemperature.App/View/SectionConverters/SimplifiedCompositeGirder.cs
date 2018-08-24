﻿using BridgeTemperature.DistributionOperations;
using BridgeTemperature.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.View.ViewClasses
{
    public class SimplifiedCompositeGirder
    {
        public double Tf1 { get; set; }
        public double Hw { get; set; }
        public double Tf2 { get; set; }
        public double Bf1 { get; set; }
        public double Bf2 { get; set; }
        public double Tw { get; set; }
        public double Hp { get; set; }
        public double Bp { get; set; }
        public double DT1 { get; set; }

        public double MaxX
        {
            get
            {
                return Math.Max(GetPlateGirderCoordinates().Max(e => e.X), GetSlabCoordinates().Max(e => e.X));
            }
        }

        public double MinX
        {
            get
            {
                return Math.Min(GetPlateGirderCoordinates().Min(e => e.X), GetSlabCoordinates().Min(e => e.X));
            }
        }

        public double MaxY
        {
            get
            {
                return Math.Max(GetPlateGirderCoordinates().Max(e => e.Y), GetSlabCoordinates().Max(e => e.Y));
            }
        }

        public double MinY
        {
            get
            {
                return Math.Min(GetPlateGirderCoordinates().Min(e => e.Y), GetSlabCoordinates().Min(e => e.Y));
            }
        }

        public SimplifiedCompositeGirder(double tf1, double hw, double tf2, double bf1,
            double bf2, double tw, double hp, double bp, double dt1)
        {
            Tf1 = tf1;
            Hw = hw;
            Tf2 = tf2;
            Bf1 = bf1;
            Bf2 = bf2;
            Tw = tw;
            Hp = hp;
            Bp = bp;
            DT1 = dt1;
        }

        public IList<PointD> GetPlateGirderCoordinates()
        {
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(Bf2, 0));
            coordinates.Add(new PointD(Bf2, Tf2));
            coordinates.Add(new PointD(Bf2 / 2 + Tw / 2, Tf2));
            coordinates.Add(new PointD(Bf2 / 2 + Tw / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 + Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 + Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 - Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 - Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 - Tw / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 - Tw / 2, Tf2));
            coordinates.Add(new PointD(0, Tf2));
            coordinates.Add(new PointD(0, 0));
            return coordinates;
        }

        public IList<PointD> GetSlabCoordinates()
        {
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(Bf2 / 2 - Bp / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 + Bp / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 + Bp / 2, Tf2 + Hw + Tf1 + Hp));
            coordinates.Add(new PointD(Bf2 / 2 - Bp / 2, Tf2 + Hw + Tf1 + Hp));
            return coordinates;
        }

        public virtual IList<Distribution> GetPlateGirderTemperature()
        {
            var distribution = new List<Distribution>();
            distribution.Add(new Distribution(0, 0));
            distribution.Add(new Distribution(Tf2 + Hw + Tf1, 0));
            return distribution;
        }

        public virtual IList<Distribution> GetSlabTemperature()
        {
            var distribution = new List<Distribution>();
            distribution.Add(new Distribution(Tf2 + Hw + Tf1, DT1));
            distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp, DT1));
            return distribution;
        }
    }
}