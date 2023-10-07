using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace FunctionToGraph.Converters;

public class ColorToBrushConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

        if (value is Color color)
        {
            return new SolidColorBrush(color);
        }
        else
        {
            throw new InvalidOperationException("Value must be a Color");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        
        throw new NotImplementedException();
    }

}