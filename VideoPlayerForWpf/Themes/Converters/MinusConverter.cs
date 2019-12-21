using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoPlayerForWpf.Themes.Converters
{
    public class MinusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is double && parameter != null)
            {
                var dValue = double.Parse(System.Convert.ToString(value));
                var minus = double.Parse(System.Convert.ToString(parameter));

                if (dValue > minus)
                {
                    return dValue - minus;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
