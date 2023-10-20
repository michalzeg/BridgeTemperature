using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BridgeTemperature.Shared.Geometry;
using BridgeTemperature.Shared.Extensions;

namespace BridgeTemperature.Calculations.Sections
{
    public class SectionPropertiesCalculations
    {
        private IList<PointD> _coordinates;
        private double _A;
        private double _Sx;
        private double _Sy;
        private double _Ix;
        private double _Iy;
        private double _Ixy;
        private double _Ix0;
        private double _Iy0;
        private double _Ixy0;
        private double _I1;
        private double _I2;
        private double _Alfa;
        private double _X0Max;
        private double _X0Min;
        private double _Y0Max;
        private double _Y0Min;
        private double _XIMax;
        private double _XIMin;
        private double _YIMax;
        private double _YIMin;
        private double _XMax;
        private double _XMin;
        private double _YMax;
        private double _YMin;
        private double _X0;
        private double _Y0;
        private double _B;
        private double _H;

        public double A
        {
            get
            {
                if (_A.IsNaN())
                    CalculateBasicProperties();
                return _A;
            }
        }

        public double Sx
        {
            get
            {
                if (_Sx.IsNaN())
                    CalculateBasicProperties();
                return _Sx;
            }
        }

        public double Sy
        {
            get
            {
                if (_Sy.IsNaN())
                    CalculateBasicProperties();
                return _Sy;
            }
        }

        public double Ix
        {
            get
            {
                if (_Ix.IsNaN())
                    CalculateBasicProperties();
                return _Ix;
            }
        }

        public double Iy
        {
            get
            {
                if (_Iy.IsNaN())
                    CalculateBasicProperties();
                return _Iy;
            }
        }

        public double Ixy
        {
            get
            {
                if (_Ixy.IsNaN())
                    CalculateBasicProperties();
                return _Ixy;
            }
        }

        public double Ix0
        {
            get
            {
                if (_Ix0.IsNaN())
                    CalculateCentralProperties();
                return _Ix0;
            }
        }

        public double Iy0
        {
            get
            {
                if (_Iy0.IsNaN())
                    CalculateCentralProperties();
                return _Iy0;
            }
        }

        public double Ixy0
        {
            get
            {
                if (_Ixy0.IsNaN())
                    CalculateCentralProperties();
                return _Ixy0;
            }
        }

        public double I1
        {
            get
            {
                if (_I1.IsNaN())
                    CalculatePrincipalProperties();
                return _I1;
            }
        }

        public double I2
        {
            get
            {
                if (_I2.IsNaN())
                    CalculatePrincipalProperties();
                return _I2;
            }
        }

        public double Alfa
        {
            get
            {
                if (_Alfa.IsNaN())
                    CalculatePrincipalProperties();
                return _Alfa;
            }
        }

        public double X0Max
        {
            get
            {
                if (_X0Max.IsNaN())
                    ExtremeDistancesCentralCoordinateSystem();
                return _X0Max;
            }
        }

        public double X0Min
        {
            get
            {
                if (_X0Min.IsNaN())
                    ExtremeDistancesCentralCoordinateSystem();
                return _X0Min;
            }
        }

        public double Y0Max
        {
            get
            {
                if (_Y0Max.IsNaN())
                    ExtremeDistancesCentralCoordinateSystem();
                return _Y0Max;
            }
        }

        public double Y0Min
        {
            get
            {
                if (_Y0Min.IsNaN())
                    ExtremeDistancesCentralCoordinateSystem();
                return _Y0Min;
            }
        }

        public double XIMax
        {
            get
            {
                if (_XIMax.IsNaN())
                    ExtremeDistancesPrincipalCoordinateSystem();
                return _XIMax;
            }
        }

        public double XIMin
        {
            get
            {
                if (_XIMin.IsNaN())
                    ExtremeDistancesPrincipalCoordinateSystem();
                return _XIMin;
            }
        }

        public double YIMax
        {
            get
            {
                if (_YIMax.IsNaN())
                    ExtremeDistancesPrincipalCoordinateSystem();
                return _YIMax;
            }
        }

        public double YIMin
        {
            get
            {
                if (_YIMin.IsNaN())
                    ExtremeDistancesPrincipalCoordinateSystem();
                return _YIMin;
            }
        }

        public double XMax
        {
            get
            {
                if (_XMax.IsNaN())
                    ExtremeCoordinates();
                return _XMax;
            }
        }

        public double XMin
        {
            get
            {
                if (_XMin.IsNaN())
                    ExtremeCoordinates();
                return _XMin;
            }
        }

        public double YMax
        {
            get
            {
                if (_YMax.IsNaN())
                    ExtremeCoordinates();
                return _YMax;
            }
        }

        public double YMin
        {
            get
            {
                if (_YMin.IsNaN())
                    ExtremeCoordinates();
                return _YMin;
            }
        }

        public double X0
        {
            get
            {
                if (_X0.IsNaN())
                    CalculateCentreOfGravity();
                return _X0;
            }
        }

        public double Y0
        {
            get
            {
                if (_Y0.IsNaN())
                    CalculateCentreOfGravity();
                return _Y0;
            }
        }

        public double B
        {
            get
            {
                if (_B.IsNaN())
                    CalculateRectangle();
                return _B;
            }
        }

        public double H
        {
            get
            {
                if (_H.IsNaN())
                    CalculateRectangle();
                return _H;
            }
        }

        public PointD CentreOfGravity
        {
            get
            {
                return new PointD(X0, Y0);
            }
        }

        public SectionPropertiesCalculations(IList<PointD> coordinates)
        {
            _coordinates = coordinates;
            SetNaNValues();
        }

        private void SetNaNValues()
        {
            _A = double.NaN;
            _Sx = double.NaN;
            _Sy = double.NaN;
            _Ix = double.NaN;
            _Iy = double.NaN;
            _Ixy = double.NaN;
            _Ix0 = double.NaN;
            _Iy0 = double.NaN;
            _Ixy0 = double.NaN;
            _I1 = double.NaN;
            _I2 = double.NaN;
            _Alfa = double.NaN;
            _X0Max = double.NaN;
            _X0Min = double.NaN;
            _Y0Max = double.NaN;
            _Y0Min = double.NaN;
            _XIMax = double.NaN;
            _XIMin = double.NaN;
            _YIMax = double.NaN;
            _YIMin = double.NaN;
            _XMax = double.NaN;
            _XMin = double.NaN;
            _YMax = double.NaN;
            _YMin = double.NaN;
            _X0 = double.NaN;
            _Y0 = double.NaN;
            _B = double.NaN;
            _H = double.NaN;
        }

        private void CalculateBasicProperties()
        {
            double a = 0;
            double sx = 0;
            double sy = 0;
            double ix = 0;
            double iy = 0;
            double ixy = 0;

            for (int i = 0; i <= _coordinates.Count - 2; i++)
            {
                double x1, x2, y1, y2;
                x1 = _coordinates[i].X;
                x2 = _coordinates[i + 1].X;
                y1 = _coordinates[i].Y;
                y2 = _coordinates[i + 1].Y;
                a = a + (x1 - x2) * (y2 + y1);
                sx = sx + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
                sy = sy + (y2 - y1) * (x1 * x1 + x1 * x2 + x2 * x2);
                ix = ix + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
                iy = iy + (y2 - y1) * (x1 * x1 * x1 + x1 * x1 * x2 + x1 * x2 * x2 + x2 * x2 * x2);
                ixy = ixy + (x1 - x2) * (x1 * (3 * y1 * y1 + y2 * y2 + 2 * y1 * y2) + x2 * (3 * y2 * y2 + y1 * y1 + 2 * y1 * y2));
            }

            a = a / 2;
            sx = sx / 6;
            sy = sy / 6;
            ix = ix / 12;
            iy = iy / 12;
            ixy = ixy / 24;

            _A = a;
            _Sx = sx;
            _Sy = sy;
            _Ix = ix;
            _Iy = iy;
            _Ixy = ixy;
        }

        private void CalculateCentreOfGravity()
        {
            if (_A.IsNaN() || _Sx.IsNaN() || _Sy.IsNaN())
                CalculateBasicProperties();
            _X0 = _Sy / _A;
            _Y0 = _Sx / _A;
        }

        private void CalculateCentralProperties()
        {
            if (_X0.IsNaN() || _Y0.IsNaN())
                CalculateCentreOfGravity();
            _Ix0 = _Ix - _A * _Y0 * _Y0;
            _Iy0 = _Iy - _A * _X0 * _X0;
            _Ixy0 = _Ixy - _A * _X0 * _Y0;
        }

        private void CalculatePrincipalProperties()
        {
            if (_Ix0.IsNaN() || _Iy0.IsNaN() || _Ixy0.IsNaN())
                CalculateCentralProperties();
            _I1 = (_Ix0 + _Iy0) / 2 + 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            _I2 = (_Ix0 + _Iy0) / 2 - 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            _Alfa = Math.Atan(_Ixy0 / (_Iy0 - _I1));
            if (double.IsNaN(_Alfa))
                _Alfa = Math.PI / 2;
        }

        private void ExtremeDistancesCentralCoordinateSystem()
        {
            if (_X0.IsNaN() || _Y0.IsNaN())
                CalculateCentreOfGravity();
            _X0Max = _coordinates.Max(point => point.X) - _X0;
            _X0Min = _coordinates.Min(point => point.X) - _X0;
            _Y0Max = _coordinates.Max(point => point.Y) - _Y0;
            _Y0Min = _coordinates.Min(point => point.Y) - _Y0;
        }

        private void ExtremeDistancesPrincipalCoordinateSystem()
        {
            if (_Alfa.IsNaN())
                CalculatePrincipalProperties();
            double cos = Math.Cos(_Alfa);
            double sin = Math.Sin(_Alfa);
            double xo = _X0 * cos - _Y0 * sin;
            double yo = _X0 * sin + _Y0 * cos;

            _XIMax = _coordinates.Max(point => point.X * cos - point.Y * sin) - xo;
            _XIMin = _coordinates.Min(point => point.X * cos - point.Y * sin) - xo;
            _YIMax = _coordinates.Max(point => point.X * sin + point.Y * cos) - yo;
            _YIMin = _coordinates.Min(point => point.X * sin + point.Y * cos) - yo;
        }

        private void ExtremeCoordinates()
        {
            _XMax = _coordinates.Max(point => point.X);
            _YMax = _coordinates.Max(point => point.Y);
            _XMin = _coordinates.Min(point => point.X);
            _YMin = _coordinates.Min(point => point.Y);
        }

        private void CalculateRectangle()
        {
            if (_Ix0.IsNaN())
                CalculateCentralProperties();
            _H = Math.Sqrt(12 * _Ix0 / _A);
            _B = _A / _H;
        }

        public Dictionary<SectionCharacteristic, double> GetAllProperties()
        {
            var allProperties = new Dictionary<SectionCharacteristic, double>
            {
                { SectionCharacteristic.Alfa, Alfa },
                { SectionCharacteristic.B, B },
                { SectionCharacteristic.A, A },
                { SectionCharacteristic.H, H },
                { SectionCharacteristic.I1, I1 },
                { SectionCharacteristic.I2, I2 },
                { SectionCharacteristic.Ix, Ix },
                { SectionCharacteristic.Ix0, Ix0 },
                { SectionCharacteristic.Ixy, Ixy },
                { SectionCharacteristic.Ixy0, Ixy0 },
                { SectionCharacteristic.Iy, Iy },
                { SectionCharacteristic.Iy0, Iy0 },
                { SectionCharacteristic.Sx, Sx },
                { SectionCharacteristic.Sy, Sy },
                { SectionCharacteristic.X0Max, X0Max },
                { SectionCharacteristic.X0Min, X0Min },
                { SectionCharacteristic.XIMax, XIMax },
                { SectionCharacteristic.XIMin, XIMin },
                { SectionCharacteristic.X0, X0 },
                { SectionCharacteristic.Y0Max, Y0Max },
                { SectionCharacteristic.Y0Min, Y0Min },
                { SectionCharacteristic.YIMax, YIMax },
                { SectionCharacteristic.YIMin, YIMin },
                { SectionCharacteristic.Y0, Y0 },
                { SectionCharacteristic.Xmax, XMax },
                { SectionCharacteristic.Xmin, XMin },
                { SectionCharacteristic.Ymax, YMax },
                { SectionCharacteristic.Ymin, YMin }
            };

            return allProperties;
        }
    }
}