using System;

using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.Views.Pickers
{
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
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
