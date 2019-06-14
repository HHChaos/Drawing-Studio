using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Helpers;
using LearnDraw.Services;
using LearnDraw.ViewModels.PickerViewModels;
using LearnDraw.Views;

using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

using WinUI = Microsoft.UI.Xaml.Controls;

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
    }
}
