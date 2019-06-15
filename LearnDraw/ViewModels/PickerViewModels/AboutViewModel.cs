using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Helpers;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class AboutViewModel : ObjectPickerBase<bool>
    {
        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            VersionDescription = GetVersionDescription();
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
