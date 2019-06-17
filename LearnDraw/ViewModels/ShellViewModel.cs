using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.ViewModels.PickerViewModels;
using System;
using System.Windows.Input;
using Windows.System;

namespace LearnDraw.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public static readonly string ContentNavigationServiceKey = "HomeShellViewModel_Content";

        public ShellViewModel()
        {
        }

        public ICommand OpenSettingsPageCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(SettingsViewModel).FullName);
                });
            }
        }

        public ICommand OpenAboutPageCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(AboutViewModel).FullName);
                });
            }
        }

        public ICommand FeedbackCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9PKVX26RB8F0"));
                });
            }
        }
    }
}
