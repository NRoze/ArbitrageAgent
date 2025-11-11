using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Converters
{
    public class SecondsToMiliConverter : IValueConverter
    {
        // Converts from model → UI
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return d / 1000.0;

            if (value is int i)
                return i / 1000.0;

            return 1000.0;
        }

        // Converts from UI → model
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && double.TryParse(s, NumberStyles.Any, culture, out double num))
            {
                if (num > 0)
                {
                    return num * 1000.0;
                }
            }

            return null;
        }
    }
}
