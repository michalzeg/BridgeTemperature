using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BridgeTemperature.Converters
{
    public class MultiplyValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = System.Convert.ToDouble(value);
            var b = System.Convert.ToDouble(parameter);
            return a * b;
            //return (double)value / (double)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = System.Convert.ToDouble(value);
            var b = System.Convert.ToDouble(parameter);
            return a / b;
        }
    }
}
