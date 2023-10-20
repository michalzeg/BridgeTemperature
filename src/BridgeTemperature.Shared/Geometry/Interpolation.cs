using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BridgeTemperature.Shared.Geometry
{
    public class Interpolation
    {
        private IList<double> _x;
        private IList<double> _y;

        public Interpolation(IEnumerable<double> x, IEnumerable<double> y)
        {
            _x = x.ToList();
            _y = y.ToList();
            if (_x.Count != _y.Count)
                throw new ArgumentException("The lists should have the same lenght");
        }

        public double Interpolate(double value)
        {
            if (_x.Count == 0 || _y.Count == 0)
                return 0;
            if (value < _x.Min() || value > _x.Max())
                return 0;
            double result = double.NaN;
            for (int i = 0; i <= _x.Count - 2; i++)
            {
                if (value <= _x[i + 1] && value >= _x[i])
                {
                    result = _y[i] + (_y[i + 1] - _y[i]) / (_x[i + 1] - _x[i]) * (value - _x[i]);
                }
            }
            return result;
        }
    }
}