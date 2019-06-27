using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace LearnDraw.Converters
{
    public class SelectionModeToVisibilityConverter : IValueConverter
    {
        public bool IsInversed { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var selectMode = (ListViewSelectionMode)value;
            return (selectMode == ListViewSelectionMode.Multiple) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
