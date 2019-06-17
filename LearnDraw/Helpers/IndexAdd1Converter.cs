using System;
using Windows.UI.Xaml.Data;

namespace LearnDraw.Helpers
{
    public class IndexAdd1Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is int) ? $"{(int)value + 1}" : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
