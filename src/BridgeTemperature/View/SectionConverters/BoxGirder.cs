using BridgeTemperature.DistributionOperations;
using BridgeTemperature.Helpers;
using System.Collections.Generic;

namespace BridgeTemperature.View.ViewClasses
{
    public class BoxGirder
    {
        public double Tf1 { get; set; }
        public double Hw { get; set; }
        public double Tf2 { get; set; }
        public double Bf1 { get; set; }
        public double Bf2 { get; set; }
        public double Tw { get; set; }
        public double DT1 { get; set; }
        public double DT3 { get; set; }
        public double H3 { get; set; }
        public double DT4 { get; set; }
        public double H4 { get; set; }
        public double DT2 { get; set; }
        public double H1 { get; set; }
        public double H2 { get; set; }

        public BoxGirder(double tf1, double hw, double tf2, double tw,
            double bf1, double bf2, double dt1, double dt2,
            double dt3, double dt4, double h1, double h2, double h3, double h4)

        {
            DT3 = dt3;
            DT4 = dt4;
            H3 = h3;
            H4 = h4;
            Tf1 = tf1;
            Hw = hw;
            Tf2 = tf2;
            Bf1 = bf1;
            Bf2 = bf2;
            Tw = tw;
            DT1 = dt1;
            DT2 = dt2;
            H1 = h1;
            H2 = h2;
        }

        public IList<PointD> GetIGirderCoordinates()
        {
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(Tw + Bf2 + Tw, 0));
            coordinates.Add(new PointD(Tw + Bf2 + Tw, Tf2 + Hw));
            coordinates.Add(new PointD(Tw + Bf2 / 2 + Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Tw + Bf2 / 2 + Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Tw + Bf2 / 2 - Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Tw + Bf2 / 2 - Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(0, Tf2 + Hw));
            coordinates.Add(new PointD(Tw, Tf2 + Hw));
            coordinates.Add(new PointD(Tw + Bf2, Tf2 + Hw));
            coordinates.Add(new PointD(Tw + Bf2, Tf2));
            coordinates.Add(new PointD(Tw, Tf2));
            coordinates.Add(new PointD(Tw, Tf2 + Hw));
            coordinates.Add(new PointD(0, Tf2 + Hw));
            coordinates.Add(new PointD(0, 0));

            return coordinates;
        }

        public IList<Distribution> GetIGirderDistribution()
        {
            var h = Tf2 + Hw + Tf1;
            var distribution = new List<Distribution>();
            distribution.Add(new Distribution(0, DT4));
            distribution.Add(new Distribution(H4, DT3));
            distribution.Add(new Distribution(H4 + H3, 0));
            distribution.Add(new Distribution(h - H1 - H2, 0));
            distribution.Add(new Distribution(h - H1, DT2));
            distribution.Add(new Distribution(h, DT1));
            return distribution;
        }
    }
}