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

        public IList<Distribution> GetTemperature()
        {
            var distributionList = new List<Distribution>();
            distributionList.Add(new Distribution(Tf2 + Hw + Tf1 - H1, 0));
            distributionList.Add(new Distribution(Tf2 + Hw + Tf1, DT1));
            return distributionList;
        }
    }

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

        public SimplifiedCompositeGirder(double tf1,double hw,double tf2,double bf1,
            double bf2,double tw,double hp,double bp,double dt1)
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
            distribution.Add(new Distribution(Tf2 + Hw + Tf1+Hp, DT1));
            return distribution;

        }


    }

    public class NormalCompositeGirder : SimplifiedCompositeGirder
    {
        public double DT2 { get; set;}
        public double H1 { get; set; }
        public double H2 { get; set; }

        public NormalCompositeGirder(double tf1,double hw,double tf2,double tw,
            double bf1,double bf2, double bp,double hp,double dt1,double dt2,
            double h1,double h2):base(tf1,hw,tf2,bf1,bf2,tw,hp,bp,dt1)
        {
            DT2 = dt2;
            H1 = h1;
            H2 = h2;
        }

        public override IList<Distribution> GetSlabTemperature()
        {
            var distribution = new List<Distribution>();
            distribution.Add(new Distribution(Tf1 + Hw + Tf2 + Hp, DT1));
            if (H1<Hp)
            {
                distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1, DT2));
                double t = DT2 * (H1 + H2 - Hp) / H2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            else
            {
                double t = (DT1 - DT2) * (H1- Hp) / (H1) + DT2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            return distribution.OrderBy(e => e.Y).ToList();
        }

        public override IList<Distribution> GetPlateGirderTemperature()
        {
            var distribution = new List<Distribution>();
            if (H1< Hp)
            {
                double t = DT2 * (H1 + H2 - Hp) / H2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            else
            {
                double t1 = (DT1 - DT2) * (H1 - Hp)/H1 + DT2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t1));
                distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1, DT2));

            }
            distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1 - H2, 0));
            return distribution;
        }
    }

    public class ConcreteIGirder
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
        public double DT3 { get; set; }
        public double H3 { get; set; }
        public double DT4 { get; set; }
        public double H4 { get; set; }
        public double DT2 { get; set; }
        public double H1 { get; set; }
        public double H2 { get; set; }

        public ConcreteIGirder(double tf1, double hw, double tf2, double tw,
            double bf1, double bf2, double bp, double hp, double dt1, double dt2, 
            double dt3,double dt4,double h1, double h2,double h3,double h4)
            
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
            Hp = hp;
            Bp = bp;
            DT1 = dt1;
            DT2 = dt2;
            H1 = h1;
            H2 = h2;
        }
        
        public IList<PointD> GetIGirderCoordinates()
        {
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(0, 0));
            coordinates.Add(new PointD(Bf2,0));
            coordinates.Add(new PointD(Bf2, Tf2));
            coordinates.Add(new PointD(Bf2 / 2 + Tw / 2, Tf2));
            coordinates.Add(new PointD(Bf2 / 2 + Tw / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 + Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 + Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 + Bp / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 + Bp / 2, Tf2 + Hw + Tf1 + Hp));
            coordinates.Add(new PointD(Bf2 / 2 - Bp / 2, Tf2 + Hw + Tf1 + Hp));
            coordinates.Add(new PointD(Bf2 / 2 - Bp / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 - Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Bf2 / 2 - Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 - Tw / 2, Tf2 + Hw));
            coordinates.Add(new PointD(Bf2 / 2 - Tw / 2, Tf2));
            coordinates.Add(new PointD(0, Tf2));
            return coordinates;
        }
        
        public IList<Distribution> GetIGirderDistribution()
        {
            var h = Tf2 + Hw + Tf1 + Hp;
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
            double bf1, double bf2,  double dt1, double dt2,
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
            coordinates.Add(new PointD(Tw+Bf2+Tw, 0));
            coordinates.Add(new PointD(Tw + Bf2 + Tw, Tf2+Hw));
            coordinates.Add(new PointD(Tw + Bf2/2 + Bf1/2, Tf2 + Hw));
            coordinates.Add(new PointD(Tw + Bf2 / 2 + Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Tw + Bf2 / 2 - Bf1 / 2, Tf2 + Hw + Tf1));
            coordinates.Add(new PointD(Tw + Bf2 / 2 - Bf1 / 2, Tf2 + Hw));
            coordinates.Add(new PointD(0, Tf2 + Hw));
            coordinates.Add(new PointD(Tw, Tf2 + Hw));
            coordinates.Add(new PointD(Tw+Bf2, Tf2 + Hw));
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
            var coordinates = new List<PointD>();
            coordinates.Add(new PointD(X - B / 2, Y - H / 2));
            coordinates.Add(new PointD(X + B / 2, Y - H / 2));
            coordinates.Add(new PointD(X + B / 2, Y + H / 2));
            coordinates.Add(new PointD(X - B / 2, Y + H / 2));

            return coordinates;
        }

        public IList<Distribution> GetTemperature()
        {
            var distributionList = new List<Distribution>();
            distributionList.Add(new Distribution(Y-H/2, DT1));
            distributionList.Add(new Distribution(Y+H/2, DT2));
            return distributionList;
        }
    }
}
