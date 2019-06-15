using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Services;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class SettingsViewModel : ObjectPickerBase<bool>
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }


        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

    }
}
