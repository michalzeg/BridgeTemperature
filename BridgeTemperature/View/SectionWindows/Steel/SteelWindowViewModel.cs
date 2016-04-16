using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BridgeTemperature.ViewModel
{
    public class SteelWindowViewModel:ViewModelBase
    {
        public SteelWindowViewModel()
        {

        }

        private void updateDrawings()
        { }

        private double tf1;
        public double Tf1
        {
            get { return tf1; }
            set
            {
                if (value!=tf1)
                {
                    tf1 = value;
                    updateDrawings();
                }
            }
        }
        private double hw;
        public double Hw
        {
            get { return hw; }
            set
            {
                if (value != hw)
                {
                    hw = value;
                    updateDrawings();
                }
            }
        }
        private double tf2;
        public double Tf2
        {
            get { return tf2; }
            set
            {
                if (value != tf2)
                {
                    tf2 = value;
                    updateDrawings();
                }
            }
        }
        private double tw;
        public double Tw
        {
            get { return tw; }
            set
            {
                if (value != tw)
                {
                    tw = value;
                    updateDrawings();
                }
            }
        }
        private double bf;
        public double Bf
        {
            get { return bf; }
            set
            {
                if (value != bf)
                {
                    bf = value;
                    updateDrawings();
                }
            }
        }
        private double dt1;
        public double DT1
        {
            get { return dt1; }
            set
            {
                if (value != dt1)
                {
                    dt1 = value;
                    updateDrawings();
                }
            }
        }
        private double h1;
        public double H1
        {
            get { return h1; }
            set
            {
                if (value != h1)
                {
                    h1 = value;
                    updateDrawings();
                }
            }
        }
    }
}
