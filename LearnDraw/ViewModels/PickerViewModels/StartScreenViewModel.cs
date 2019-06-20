using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class StartScreenViewModel : ObjectPickerBase<bool>
    {
        public bool IsShowWelcomeScreen
        {
            get => ApplicationData.Current.LocalSettings.ReadBool(SettingsContract.IsShowWelcomeScreen);
            set
            {
                ApplicationData.Current.LocalSettings.SaveBool(SettingsContract.IsShowWelcomeScreen, value);
                RaisePropertyChanged(() => IsShowWelcomeScreen);
            }
        }
    }
}
