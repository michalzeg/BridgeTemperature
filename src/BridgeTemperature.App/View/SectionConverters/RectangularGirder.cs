using BridgeTemperature.DistributionOperations;
using BridgeTemperature.Helpers;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTemperature.View.ViewClasses
{
    public class RectangularGirder
    {
        public double B { get; set; }
        public double H { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public double DT1 { get; set; }
        public double DT2 { get; set; }

        public RectangularGirder(double b, double h, double x, double y,
             double dt1, double dt2)
        {
            B = b;
            H = h;
            X = x;
            Y = y;
            DT1 = dt1;
            DT2 = dt2;
        }

        public IList<PointD> GetCoordinates()
        {
            var coordinates = new List<PointD>
            {
                new PointD(X - B / 2, Y - H / 2),
                new PointD(X + B / 2, Y - H / 2),
                new PointD(X + B / 2, Y + H / 2),
                new PointD(X - B / 2, Y + H / 2)
            };

            return coordinates;
        }

        public IList<Distribution> GetTemperature()
        {
            var distributionList = new List<Distribution>
            {
                new Distribution(Y - H / 2, DT1),
                new Distribution(Y + H / 2, DT2)
            };
            return distributionList;
        }
    }
}