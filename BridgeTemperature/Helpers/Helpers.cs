using BridgeTemperature.Common;
using BridgeTemperature.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BridgeTemperature.Helpers
{
	public class PointD :IEquatable<PointD>
	{
		public double X { get; set; }
		public double Y { get; set; }
		public PointD(double x, double y)
		{

			this.X = x;
			this.Y = y;
		}
		public PointD()
		{ }

		public bool Equals(PointD other)
		{
			
			if (Object.ReferenceEquals(other, null)) return false;
			if (Object.ReferenceEquals(this, other)) return true;
			
			return X.IsApproximatelyEqualTo(other.X) && Y.IsApproximatelyEqualTo(other.Y);
		}
		public override int GetHashCode()
		{
			
			int hashY = X.GetHashCode();
			int hashValue = Y.GetHashCode();
			return hashY ^ hashValue;
		}
	}

	public class Interpolation
	{
		private IList<double> x;
		private IList<double> y;

		public Interpolation(IEnumerable<double> x, IEnumerable<double> y)
		{
			
			this.x = x.ToList();
			this.y = y.ToList();
			if (this.x.Count != this.y.Count)
				throw new ArgumentException("The lists should have the same lenght");

        }
		public double Interpolate(double value)
		{
            if (this.x.Count == 0 || this.y.Count == 0)
                return 0;
			if (value < this.x.Min() || value > this.x.Max())
				return 0;
			double result = double.NaN;
			for (int i = 0; i <= this.x.Count - 2; i++)
			{
				if (value <= x[i+1] && value >= x[i])
				{
					result = y[i] + (y[i + 1] - y[i]) / (x[i + 1] - x[i]) * (value - x[i]);
				}
			}
			return result;
		}

	}
}
