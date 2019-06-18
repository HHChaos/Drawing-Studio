using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.ViewModels.PickerViewModels;
using System.Windows.Input;

namespace LearnDraw.ViewModels
{
    public class AnimDrawingViewModel : ViewModelBase
    {
        public AnimDrawingViewModel()
        {
        }

        private int _speed = 5;
        public int Speed
        {
            get => _speed;
            set => Set(ref _speed, value);
        }
        public ICommand OpenPlayerSettingsCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<AnimConfig>(typeof(AnimSettingsViewModel).FullName, new AnimConfig { PlaySpeed = Speed });
                    if (!result.Canceled)
                    {
                        Speed = result.Result.PlaySpeed;
                    }
                });
            }
        }
    }
}
