using System;
using System.Globalization;
using Xamarin.Forms;

namespace AirMonitor.Converters
{
    public class PercentToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !double.TryParse(value?.ToString(), out var result) ? value : (result / 100);
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
            throw new NotImplementedException();
    }
}