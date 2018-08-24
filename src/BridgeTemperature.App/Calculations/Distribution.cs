using System;
using System.Collections;
using BridgeTemperature.Helpers;
using BridgeTemperature.Extensions;
using BridgeTemperature.Common;

namespace BridgeTemperature.DistributionOperations
{
    public class Distribution : IEquatable<Distribution>
    {
        public double Y { get; set; }
        public double Value { get; set; }

        public Distribution()
        { }

        public Distribution(double y, double value)
        {
            Y = y;
            Value = value;
        }

        public PointD ConvertToPointD()
        {
            return new PointD(Value, Y);
        }

        public bool Equals(Distribution other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return Y.IsApproximatelyEqualTo(other.Y) && Value.IsApproximatelyEqualTo(other.Value);
        }

        public override int GetHashCode()
        {
            int hashY = Y.GetHashCode();

            int hashValue = Value.GetHashCode();

            return hashY ^ hashValue;
        }
    }
}