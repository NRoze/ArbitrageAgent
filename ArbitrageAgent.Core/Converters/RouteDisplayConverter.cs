using ArbitrageAgent.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbitrageAgent.Core.Converters
{
    public class RouteDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Expect a tuple (List<AssetNode> Route, decimal ProfitRate)
            if (value is ValueTuple<List<AssetNode>, decimal> tuple)
            {
                var route = tuple.Item1;
                return $"Profit: {tuple.Item2-1:P3}   [ {string.Join(" → ", route.Select(n => $"{n.Name} ({n.ExchangeName})"))} ] ";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
