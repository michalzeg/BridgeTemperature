using BridgeTemperature.Helpers;
using BridgeTemperature.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BridgeTemperature.Extensions;

namespace BridgeTemperature.SectionProperties
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
                    this.calculateBasicProperties();
                return _A;
            }
        }

        public double Sx
        {
            get
            {
                if (_Sx.IsNaN())
                    this.calculateBasicProperties();
                return _Sx;
            }
        }

        public double Sy
        {
            get
            {
                if (_Sy.IsNaN())
                    this.calculateBasicProperties();
                return _Sy;
            }
        }

        public double Ix
        {
            get
            {
                if (_Ix.IsNaN())
                    this.calculateBasicProperties();
                return _Ix;
            }
        }

        public double Iy
        {
            get
            {
                if (_Iy.IsNaN())
                    this.calculateBasicProperties();
                return _Iy;
            }
        }

        public double Ixy
        {
            get
            {
                if (_Ixy.IsNaN())
                    this.calculateBasicProperties();
                return _Ixy;
            }
        }

        public double Ix0
        {
            get
            {
                if (_Ix0.IsNaN())
                    this.calculateCentralProperties();
                return _Ix0;
            }
        }

        public double Iy0
        {
            get
            {
                if (_Iy0.IsNaN())
                    this.calculateCentralProperties();
                return _Iy0;
            }
        }

        public double Ixy0
        {
            get
            {
                if (_Ixy0.IsNaN())
                    this.calculateCentralProperties();
                return _Ixy0;
            }
        }

        public double I1
        {
            get
            {
                if (_I1.IsNaN())
                    this.calculatePrincipalProperties();
                return _I1;
            }
        }

        public double I2
        {
            get
            {
                if (_I2.IsNaN())
                    this.calculatePrincipalProperties();
                return _I2;
            }
        }

        public double Alfa
        {
            get
            {
                if (_Alfa.IsNaN())
                    this.calculatePrincipalProperties();
                return _Alfa;
            }
        }

        public double X0Max
        {
            get
            {
                if (_X0Max.IsNaN())
                    this.extremeDistancesCentralCoordinateSystem();
                return _X0Max;
            }
        }

        public double X0Min
        {
            get
            {
                if (_X0Min.IsNaN())
                    this.extremeDistancesCentralCoordinateSystem();
                return _X0Min;
            }
        }

        public double Y0Max
        {
            get
            {
                if (_Y0Max.IsNaN())
                    this.extremeDistancesCentralCoordinateSystem();
                return _Y0Max;
            }
        }

        public double Y0Min
        {
            get
            {
                if (_Y0Min.IsNaN())
                    this.extremeDistancesCentralCoordinateSystem();
                return _Y0Min;
            }
        }

        public double XIMax
        {
            get
            {
                if (_XIMax.IsNaN())
                    this.extremeDistancesPrincipalCoordinateSystem();
                return _XIMax;
            }
        }

        public double XIMin
        {
            get
            {
                if (_XIMin.IsNaN())
                    this.extremeDistancesPrincipalCoordinateSystem();
                return _XIMin;
            }
        }

        public double YIMax
        {
            get
            {
                if (_YIMax.IsNaN())
                    this.extremeDistancesPrincipalCoordinateSystem();
                return _YIMax;
            }
        }

        public double YIMin
        {
            get
            {
                if (_YIMin.IsNaN())
                    this.extremeDistancesPrincipalCoordinateSystem();
                return _YIMin;
            }
        }

        public double XMax
        {
            get
            {
                if (_XMax.IsNaN())
                    this.extremeCoordinates();
                return _XMax;
            }
        }

        public double XMin
        {
            get
            {
                if (_XMin.IsNaN())
                    this.extremeCoordinates();
                return _XMin;
            }
        }

        public double YMax
        {
            get
            {
                if (_YMax.IsNaN())
                    this.extremeCoordinates();
                return _YMax;
            }
        }

        public double YMin
        {
            get
            {
                if (_YMin.IsNaN())
                    this.extremeCoordinates();
                return _YMin;
            }
        }

        public double X0
        {
            get
            {
                if (_X0.IsNaN())
                    this.calculateCentreOfGravity();
                return _X0;
            }
        }

        public double Y0
        {
            get
            {
                if (_Y0.IsNaN())
                    this.calculateCentreOfGravity();
                return _Y0;
            }
        }

        public double B
        {
            get
            {
                if (_B.IsNaN())
                    this.calculateRectangle();
                return _B;
            }
        }

        public double H
        {
            get
            {
                if (_H.IsNaN())
                    this.calculateRectangle();
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
            this.setNaNValues();
        }

        private void setNaNValues()
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

        private void calculateBasicProperties()
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

        private void calculateCentreOfGravity()
        {
            if (_A.IsNaN() || _Sx.IsNaN() || _Sy.IsNaN())
                this.calculateBasicProperties();
            this._X0 = _Sy / _A;
            this._Y0 = _Sx / _A;
        }

        private void calculateCentralProperties()
        {
            if (this._X0.IsNaN() || this._Y0.IsNaN())
                this.calculateCentreOfGravity();
            this._Ix0 = this._Ix - this._A * this._Y0 * this._Y0;
            this._Iy0 = this._Iy - this._A * this._X0 * this._X0;
            this._Ixy0 = this._Ixy - this._A * this._X0 * this._Y0;
        }

        private void calculatePrincipalProperties()
        {
            if (_Ix0.IsNaN() || _Iy0.IsNaN() || _Ixy0.IsNaN())
                this.calculateCentralProperties();
            this._I1 = (_Ix0 + _Iy0) / 2 + 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            this._I2 = (_Ix0 + _Iy0) / 2 - 0.5 * Math.Sqrt(Math.Pow(_Iy0 - _Ix0, 2) + 4 * _Ixy0 * _Ixy0);
            this._Alfa = Math.Atan(_Ixy0 / (_Iy0 - _I1));
            if (double.IsNaN(_Alfa))
                this._Alfa = Math.PI / 2;
        }

        private void extremeDistancesCentralCoordinateSystem()
        {
            if (_X0.IsNaN() || _Y0.IsNaN())
                this.calculateCentreOfGravity();
            _X0Max = coordinates.Max(point => point.X) - this._X0;
            _X0Min = coordinates.Min(point => point.X) - this._X0;
            _Y0Max = coordinates.Max(point => point.Y) - this._Y0;
            _Y0Min = coordinates.Min(point => point.Y) - this._Y0;
        }

        private void extremeDistancesPrincipalCoordinateSystem()
        {
            if (_Alfa.IsNaN())
                this.calculatePrincipalProperties();
            double cos = Math.Cos(_Alfa);
            double sin = Math.Sin(_Alfa);
            double xo = _X0 * cos - _Y0 * sin;
            double yo = _X0 * sin + _Y0 * cos;

            _XIMax = coordinates.Max(point => point.X * cos - point.Y * sin) - xo;
            _XIMin = coordinates.Min(point => point.X * cos - point.Y * sin) - xo;
            _YIMax = coordinates.Max(point => point.X * sin + point.Y * cos) - yo;
            _YIMin = coordinates.Min(point => point.X * sin + point.Y * cos) - yo;
        }

        private void extremeCoordinates()
        {
            _XMax = coordinates.Max(point => point.X);
            _YMax = coordinates.Max(point => point.Y);
            _XMin = coordinates.Min(point => point.X);
            _YMin = coordinates.Min(point => point.Y);
        }

        private void calculateRectangle()
        {
            if (_Ix0.IsNaN())
                this.calculateCentralProperties();
            _H = Math.Sqrt(12 * _Ix0 / _A);
            _B = _A / _H;
        }

        [Obsolete]
        private void CalculateProperties()
        {
            double F = 0;
            double Sx = 0;
            double Sy = 0;
            double Ix = 0;
            double Iy = 0;
            double Ixy = 0;

            for (int i = 0; i <= coordinates.Count - 2; i++)
            {
                double x1, x2, y1, y2;
                x1 = coordinates[i].X;
                x2 = coordinates[i + 1].X;
                y1 = coordinates[i].Y;
                y2 = coordinates[i + 1].Y;
                F = F + (x1 - x2) * (y2 + y1);
                Sx = Sx + (x1 - x2) * (y1 * y1 + y1 * y2 + y2 * y2);
                Sy = Sy + (y2 - y1) * (x1 * x1 + x1 * x2 + x2 * x2);
                Ix = Ix + (x1 - x2) * (y1 * y1 * y1 + y1 * y1 * y2 + y1 * y2 * y2 + y2 * y2 * y2);
                Iy = Iy + (y2 - y1) * (x1 * x1 * x1 + x1 * x1 * x2 + x1 * x2 * x2 + x2 * x2 * x2);
                Ixy = Ixy + (x1 - x2) * (x1 * (3 * y1 * y1 + y2 * y2 + 2 * y1 * y2) + x2 * (3 * y2 * y2 + y1 * y1 + 2 * y1 * y2));
            }
            F = F / 2;
            Sx = Sx / 6;
            Sy = Sy / 6;
            Ix = Ix / 12;
            Iy = Iy / 12;
            Ixy = Ixy / 24;
            double x0 = Sy / F;
            double y0 = Sx / F;
            double Ix0 = Ix - F * y0 * y0;
            double Iy0 = Iy - F * x0 * x0;
            double Ixy0 = Ixy - F * x0 * y0;
            double I1 = (Ix0 + Iy0) / 2 + 0.5 * Math.Sqrt(Math.Pow(Iy0 - Ix0, 2) + 4 * Ixy0 * Ixy0);
            double I2 = (Ix0 + Iy0) / 2 - 0.5 * Math.Sqrt(Math.Pow(Iy0 - Ix0, 2) + 4 * Ixy0 * Ixy0);
            double h = Math.Sqrt(12 * Ix0 / F);
            double b = F / h;
            double alfa = Math.Atan(Ixy0 / (Iy0 - I1));
            if (double.IsNaN(alfa))
                alfa = Math.PI / 2;

            ExtremeDistances extremeCoordinates = new ExtremeDistances(new PointD(x0, y0));
            double x0_max, x0_min, y0_max, y0_min;
            double xI_max, xI_min, yI_max, yI_min;
            double xmax, xmin, ymax, ymin;
            extremeCoordinates.MaxDistancesCentralCoordinateSystem(coordinates, out x0_max, out x0_min, out y0_max, out y0_min);
            extremeCoordinates.MaxDistancesPrincipalCoordinateSystem(coordinates, alfa, out xI_max, out xI_min, out yI_max, out yI_min);
            extremeCoordinates.ExtremeCoordinates(coordinates, out xmax, out xmin, out ymax, out ymin);
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