using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Helpers;
using LearnDraw.Services;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class SettingsViewModel : ObjectPickerBase<bool>
    {
        public Action ThemeChanged { get; set; }

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        public bool IsShowWelcomeScreen
        {
            get => ApplicationData.Current.LocalSettings.ReadBool("IsShowWelcomeScreen");
            set
            {
                ApplicationData.Current.LocalSettings.SaveBool("IsShowWelcomeScreen", value);
                RaisePropertyChanged(() => IsShowWelcomeScreen);
            }
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
                            ThemeChanged?.Invoke();
                        });
                }

                return _switchThemeCommand;
            }
        }

        public ICommand ResetCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(UnpackResViewModel).FullName);
                });
            }
        }

    }
}
