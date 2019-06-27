using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace LearnDraw.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public bool IsInversed { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (value is Visibility && (Visibility)value == Visibility.Visible) ^ IsInversed;
        }
    }
}
