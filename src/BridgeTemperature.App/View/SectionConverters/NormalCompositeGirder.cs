using BridgeTemperature.Calculations.Distributions;
using System.Collections.Generic;
using System.Linq;

namespace BridgeTemperature.View.ViewClasses
{
    public class NormalCompositeGirder : SimplifiedCompositeGirder
    {
        public double DT2 { get; set; }
        public double H1 { get; set; }
        public double H2 { get; set; }

        public NormalCompositeGirder(double tf1, double hw, double tf2, double tw,
            double bf1, double bf2, double bp, double hp, double dt1, double dt2,
            double h1, double h2) : base(tf1, hw, tf2, bf1, bf2, tw, hp, bp, dt1)
        {
            DT2 = dt2;
            H1 = h1;
            H2 = h2;
        }

        public override IList<Distribution> GetSlabTemperature()
        {
            var distribution = new List<Distribution>();
            distribution.Add(new Distribution(Tf1 + Hw + Tf2 + Hp, DT1));
            if (H1 < Hp)
            {
                distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1, DT2));
                double t = DT2 * (H1 + H2 - Hp) / H2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            else
            {
                double t = (DT1 - DT2) * (H1 - Hp) / (H1) + DT2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            return distribution.OrderBy(e => e.Y).ToList();
        }

        public override IList<Distribution> GetPlateGirderTemperature()
        {
            var distribution = new List<Distribution>();
            if (H1 < Hp)
            {
                double t = DT2 * (H1 + H2 - Hp) / H2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t));
            }
            else
            {
                double t1 = (DT1 - DT2) * (H1 - Hp) / H1 + DT2;
                distribution.Add(new Distribution(Tf2 + Hw + Tf1, t1));
                distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1, DT2));
            }
            distribution.Add(new Distribution(Tf2 + Hw + Tf1 + Hp - H1 - H2, 0));
            return distribution;
        }
    }
}