using BridgeTemperature.DistributionOperations;
using BridgeTemperature.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public SteelPlateGirder(double tf1,double hw,double tf2,double bf,
            double tw,double h1,double dt1)
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
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(Bf,0));
            coordinates.Add(new PointD(Bf,Tf2));
            coordinates.Add(new PointD(0.5*Bf+0.5*Tw,Tf2));
            coordinates.Add(new PointD(0.5*Bf+0.5*Tw,Tf2+Hw));
            coordinates.Add(new PointD(Bf,Tf2+Hw));
            coordinates.Add(new PointD(Bf,Tf2+Hw+Tf1));
            coordinates.Add(new PointD(0,Tf2+Hw+Tf1));
            coordinates.Add(new PointD(0,Tf2+Hw));
            coordinates.Add(new PointD(0.5*Bf-0.5*Tw,Tf2+Hw));
            coordinates.Add(new PointD(0.5*Bf-0.5*Tw,Tf2));
            coordinates.Add(new PointD(0,Tf2));
            coordinates.Add(new PointD(0,0));

            return coordinates;
        }

        public IList<Distribution> GetDistribution()
        {
            var distributionList = new List<Distribution>();
            distributionList.Add(new Distribution(Tf2 + Hw + Tf1 - H1, 0));
            distributionList.Add(new Distribution(Tf2 + Hw + Tf1, DT1));
            return distributionList;
        }
    }
}
