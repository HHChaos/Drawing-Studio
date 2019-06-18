using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class AnimSettingsViewModel : ObjectPickerBase<AnimConfig>
    {
        private int _speed;
        public int Speed
        {
            get => _speed;
            set => Set(ref _speed, value);
        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is AnimConfig config)
            {
                Speed = config.PlaySpeed;
            }
        }

        public ICommand SubmitCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SetResult(new AnimConfig
                    {
                        PlaySpeed = Speed
                    });
                });
            }
        }
    }
}
