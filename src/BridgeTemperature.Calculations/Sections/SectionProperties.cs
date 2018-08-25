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
        private IList<PointD> coordinates;
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
                    this.CalculateBasicProperties();
                return _A;
            }
        }

        public double Sx
        {
            get
            {
                if (_Sx.IsNaN())
                    this.CalculateBasicProperties();
                return _Sx;
            }
        }

        public double Sy
        {
            get
            {
                if (_Sy.IsNaN())
                    this.CalculateBasicProperties();
                return _Sy;
            }
        }

        public double Ix
        {
            get
            {
                if (_Ix.IsNaN())
                    this.CalculateBasicProperties();
                return _Ix;
            }
        }

        public double Iy
        {
            get
            {
                if (_Iy.IsNaN())
                    this.CalculateBasicProperties();
                return _Iy;
            }
        }

        public double Ixy
        {
            get
            {
                if (_Ixy.IsNaN())
                    this.CalculateBasicProperties();
                return _Ixy;
            }
        }

        public double Ix0
        {
            get
            {
                if (_Ix0.IsNaN())
                    this.CalculateCentralProperties();
                return _Ix0;
            }
        }

        public double Iy0
        {
            get
            {
                if (_Iy0.IsNaN())
                    this.CalculateCentralProperties();
                return _Iy0;
            }
        }

        public double Ixy0
        {
            get
            {
                if (_Ixy0.IsNaN())
                    this.CalculateCentralProperties();
                return _Ixy0;
            }
        }

        public double I1
        {
            get
            {
                if (_I1.IsNaN())
                    this.CalculatePrincipalProperties();
                return _I1;
            }
        }

        public double I2
        {
            get
            {
                if (_I2.IsNaN())
                    this.CalculatePrincipalProperties();
                return _I2;
            }
        }

        public double Alfa
        {
            get
            {
                if (_Alfa.IsNaN())
                    this.CalculatePrincipalProperties();
                return _Alfa;
            }
        }

        public double X0Max
        {
            get
            {
                if (_X0Max.IsNaN())
                    this.ExtremeDistancesCentralCoordinateSystem();
                return _X0Max;
            }
        }

        public double X0Min
        {
            get
            {
                if (_X0Min.IsNaN())
                    this.ExtremeDistancesCentralCoordinateSystem();
                return _X0Min;
            }
        }

        public double Y0Max
        {
            get
            {
                if (_Y0Max.IsNaN())
                    this.ExtremeDistancesCentralCoordinateSystem();
                return _Y0Max;
            }
        }

        public double Y0Min
        {
            get
            {
                if (_Y0Min.IsNaN())
                    this.ExtremeDistancesCentralCoordinateSystem();
                return _Y0Min;
            }
        }

        public double XIMax
        {
            get
            {
                if (_XIMax.IsNaN())
                    this.ExtremeDistancesPrincipalCoordinateSystem();
                return _XIMax;
            }
        }

        public double XIMin
        {
            get
            {
                if (_XIMin.IsNaN())
                    this.ExtremeDistancesPrincipalCoordinateSystem();
                return _XIMin;
            }
        }

        public double YIMax
        {
            get
            {
                if (_YIMax.IsNaN())
                    this.ExtremeDistancesPrincipalCoordinateSystem();
                return _YIMax;
            }
        }

        public double YIMin
        {
            get
            {
                if (_YIMin.IsNaN())
                    this.ExtremeDistancesPrincipalCoordinateSystem();
                return _YIMin;
            }
        }

        public double XMax
        {
            get
            {
                if (_XMax.IsNaN())
                    this.ExtremeCoordinates();
                return _XMax;
            }
        }

        public double XMin
        {
            get
            {
                if (_XMin.IsNaN())
                    this.ExtremeCoordinates();
                return _XMin;
            }
        }

        public double YMax
        {
            get
            {
                if (_YMax.IsNaN())
                    this.ExtremeCoordinates();
                return _YMax;
            }
        }

        public double YMin
        {
            get
            {
                if (_YMin.IsNaN())
                    this.ExtremeCoordinates();
                return _YMin;
            }
        }

        public double X0
        {
            get
            {
                if (_X0.IsNaN())
                    this.CalculateCentreOfGravity();
                return _X0;
            }
        }

        public double Y0
        {
            get
            {
                if (_Y0.IsNaN())
                    this.CalculateCentreOfGravity();
                return _Y0;
            }
        }

        public double B
        {
            get
            {
                if (_B.IsNaN())
                    this.CalculateRectangle();
                return _B;
            }
        }

        public double H
        {
            get
            {
                if (_H.IsNaN())
                    this.CalculateRectangle();
                return _H;
            }
        }

        public PointD CentreOfGravity
        {
            get
            {
                return new PointD(this.X0, this.Y0);
            }
        }

        public SectionPropertiesCalculations(IList<PointD> coordinates)
        {
            this.coordinates = coordinates;
            this.SetNaNValues();
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

            for (int i = 0; i <= coordinates.Count - 2; i++)
            {
                double x1, x2, y1, y2;
                x1 = coordinates[i].X;
                x2 = coordinates[i + 1].X;
                y1 = coordinates[i].Y;
                y2 = coordinates[i + 1].Y;
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

            this._A = a;
            this._Sx = sx;
            this._Sy = sy;
            this._Ix = ix;
            this._Iy = iy;
            this._Ixy = ixy;
        }

        private void CalculateCentreOfGravity()
        {
            if (_A.IsNaN() || _Sx.IsNaN() || _Sy.IsNaN())
                this.CalculateBasicProperties();
            this._X0 = _Sy / _A;
            this._Y0 = _Sx / _A;
        }

        private void CalculateCentralProperties()
        {
            if (this._X0.IsNaN() || this._Y0.IsNaN())
                this.CalculateCentreOfGravity();
            this._Ix0 = this._Ix - this._A * this._Y0 * this._Y0;
            this._Iy0 = this._Iy - this._A * this._X0 * this._X0;
            this._Ixy0 = this._Ixy - this._A * this._X0 * this._Y0;
        }

        private void CalculatePrincipalProperties()
        {
            if (_Ix0.IsNaN() || _Iy0.IsNaN() || _Ixy0.IsNaN())
                this.CalculateCentralProperties();
            this._I1 = (_Ix0 + _Iy0) / 2 + 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            this._I2 = (_Ix0 + _Iy0) / 2 - 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            this._Alfa = Math.Atan(_Ixy0 / (_Iy0 - _I1));
            if (double.IsNaN(_Alfa))
                this._Alfa = Math.PI / 2;
        }

        private void ExtremeDistancesCentralCoordinateSystem()
        {
            if (_X0.IsNaN() || _Y0.IsNaN())
                this.CalculateCentreOfGravity();
            _X0Max = coordinates.Max(point => point.X) - this._X0;
            _X0Min = coordinates.Min(point => point.X) - this._X0;
            _Y0Max = coordinates.Max(point => point.Y) - this._Y0;
            _Y0Min = coordinates.Min(point => point.Y) - this._Y0;
        }

        private void ExtremeDistancesPrincipalCoordinateSystem()
        {
            if (_Alfa.IsNaN())
                this.CalculatePrincipalProperties();
            double cos = Math.Cos(_Alfa);
            double sin = Math.Sin(_Alfa);
            double xo = _X0 * cos - _Y0 * sin;
            double yo = _X0 * sin + _Y0 * cos;

            _XIMax = coordinates.Max(point => point.X * cos - point.Y * sin) - xo;
            _XIMin = coordinates.Min(point => point.X * cos - point.Y * sin) - xo;
            _YIMax = coordinates.Max(point => point.X * sin + point.Y * cos) - yo;
            _YIMin = coordinates.Min(point => point.X * sin + point.Y * cos) - yo;
        }

        private void ExtremeCoordinates()
        {
            _XMax = coordinates.Max(point => point.X);
            _YMax = coordinates.Max(point => point.Y);
            _XMin = coordinates.Min(point => point.X);
            _YMin = coordinates.Min(point => point.Y);
        }

        private void CalculateRectangle()
        {
            if (_Ix0.IsNaN())
                this.CalculateCentralProperties();
            _H = Math.Sqrt(12 * _Ix0 / _A);
            _B = _A / _H;
        }

        public Dictionary<SectionCharacteristic, double> GetAllProperties()
        {
            var allProperties = new Dictionary<SectionCharacteristic, double>();
            allProperties = new Dictionary<SectionCharacteristic, double>();
            allProperties.Add(SectionCharacteristic.Alfa, Alfa);
            allProperties.Add(SectionCharacteristic.B, B);
            allProperties.Add(SectionCharacteristic.A, A);
            allProperties.Add(SectionCharacteristic.H, H);
            allProperties.Add(SectionCharacteristic.I1, I1);
            allProperties.Add(SectionCharacteristic.I2, I2);
            allProperties.Add(SectionCharacteristic.Ix, Ix);
            allProperties.Add(SectionCharacteristic.Ix0, Ix0);
            allProperties.Add(SectionCharacteristic.Ixy, Ixy);
            allProperties.Add(SectionCharacteristic.Ixy0, Ixy0);
            allProperties.Add(SectionCharacteristic.Iy, Iy);
            allProperties.Add(SectionCharacteristic.Iy0, Iy0);
            allProperties.Add(SectionCharacteristic.Sx, Sx);
            allProperties.Add(SectionCharacteristic.Sy, Sy);
            allProperties.Add(SectionCharacteristic.X0Max, X0Max);
            allProperties.Add(SectionCharacteristic.X0Min, X0Min);
            allProperties.Add(SectionCharacteristic.XIMax, XIMax);
            allProperties.Add(SectionCharacteristic.XIMin, XIMin);
            allProperties.Add(SectionCharacteristic.X0, X0);
            allProperties.Add(SectionCharacteristic.Y0Max, Y0Max);
            allProperties.Add(SectionCharacteristic.Y0Min, Y0Min);
            allProperties.Add(SectionCharacteristic.YIMax, YIMax);
            allProperties.Add(SectionCharacteristic.YIMin, YIMin);
            allProperties.Add(SectionCharacteristic.Y0, Y0);
            allProperties.Add(SectionCharacteristic.Xmax, XMax);
            allProperties.Add(SectionCharacteristic.Xmin, XMin);
            allProperties.Add(SectionCharacteristic.Ymax, YMax);
            allProperties.Add(SectionCharacteristic.Ymin, YMin);

            return allProperties;
        }
    }
}