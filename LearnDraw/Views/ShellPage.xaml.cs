using HHChaosToolkit.UWP.Picker;
using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.MLHelpers;
using LearnDraw.Services;
using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace LearnDraw.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel
        {
            get { return ViewModelLocator.Current.ShellViewModel; }
        }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            Loaded += ShellPage_Loaded;
        }

        private async void ShellPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            NavigationServiceList.Instance.RegisterOrUpdateFrame(ShellViewModel.ContentNavigationServiceKey, shellFrame);
            NavigationServiceList.Instance[ShellViewModel.ContentNavigationServiceKey].Navigate(typeof(MainViewModel).FullName);
            ShowStartScreenIfAppropriate();
            MLHelper.Instance.Init();
            await MyFavoriteAssetsHelper.Instance.Init();
            try
            {
                var assetsListFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appdata:///local/AnimAssets/assetList.json"));
                await AnimAssetsHelper.Instance.Init(assetsListFile);
            }
            catch (Exception)
            {
                //Ignored, the resource may not have been decompressed for the first time
            }
        }
        private void ShowStartScreenIfAppropriate()
        {
            if (FirstRunDisplayService.IsFirstRun)
            {
                InitSettings();
            }
            if (ViewModel.IsShowWelcomeScreen)
            {
                ViewModel.OpenStartScreenCommand?.Execute(null);
            }
        }
        private void InitSettings()
        {
            ApplicationData.Current.LocalSettings.SaveBool(SettingsContract.IsShowWelcomeScreen, true);
            ApplicationData.Current.LocalSettings.SaveInt(SettingsContract.AnimPlaySpeed, 3);
        }
    }
}
