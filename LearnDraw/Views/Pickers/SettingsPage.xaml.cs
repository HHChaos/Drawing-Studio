using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LearnDraw.Views.Pickers
{
    public sealed partial class SettingsPage : Page
    {
        private SettingsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.SettingsViewModel; }
        }

        public SettingsPage()
        {
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.ThemeChanged = () => { RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme; };
        }
    }
}
