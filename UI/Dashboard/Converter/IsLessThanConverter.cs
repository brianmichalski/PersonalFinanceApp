using System.Globalization;
using System.Windows.Data;

namespace PersonalFinanceApp.UI.Dashboard.Converter
{
    // source: https://stackoverflow.com/questions/71779176/xaml-datagrid-set-rowstyle-if-value-greater-than
    public class IsLessThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double intValue = (double)value;
            double compareToValue = Double.Parse(parameter.ToString() ?? string.Empty);

            return (intValue < compareToValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
