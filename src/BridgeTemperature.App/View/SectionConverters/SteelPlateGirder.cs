using BridgeTemperature.Calculations.Distributions;
using BridgeTemperature.Common.Geometry;
using System.Collections.Generic;

namespace BridgeTemperature.View.ViewClasses
{
    public class SteelPlateGirder
    {
        public double Tf1 { get; set; }
        public double Hw { get; set; }
        public double Tf2 { get; set; }
        public double Bf { get; set; }
        public double Tw { get; set; }
        public double H1 { get; set; }
        public double DT1 { get; set; }

        public SteelPlateGirder()
        {
        }

        public SteelPlateGirder(double tf1, double hw, double tf2, double bf,
            double tw, double h1, double dt1)
        {
            Tf1 = tf1;
            Hw = hw;
            Tf2 = tf2;
            Bf = bf;
            Tw = tw;
            H1 = h1;
            DT1 = dt1;
        }

        public IList<PointD> GetCoordinates()
        {
            var coordinates = new List<PointD>
            {
                new PointD(0, 0),
                new PointD(Bf, 0),
                new PointD(Bf, Tf2),
                new PointD(0.5 * Bf + 0.5 * Tw, Tf2),
                new PointD(0.5 * Bf + 0.5 * Tw, Tf2 + Hw),
                new PointD(Bf, Tf2 + Hw),
                new PointD(Bf, Tf2 + Hw + Tf1),
                new PointD(0, Tf2 + Hw + Tf1),
                new PointD(0, Tf2 + Hw),
                new PointD(0.5 * Bf - 0.5 * Tw, Tf2 + Hw),
                new PointD(0.5 * Bf - 0.5 * Tw, Tf2),
                new PointD(0, Tf2),
                new PointD(0, 0)
            };

            return coordinates;
        }

        public IList<Distribution> GetTemperature()
        {
            var distributionList = new List<Distribution>
            {
                new Distribution(Tf2 + Hw + Tf1 - H1, 0),
                new Distribution(Tf2 + Hw + Tf1, DT1)
            };
            return distributionList;
        }
    }
}