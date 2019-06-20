using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.ViewModels.PickerViewModels;
using System;
using System.Windows.Input;

namespace LearnDraw.ViewModels
{
    public class AnimDrawingViewModel : ViewModelBase
    {
        public AnimDrawingViewModel()
        {
        }

        private int _speed = 3;
        public int Speed
        {
            get => _speed;
            set => Set(ref _speed, value);
        }

        private bool _isFavorite = false;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                Set(ref _isFavorite, value);
                if (value)
                {
                    ToastHelper.SendFavoriteToast("It has been added to my favorite drawings!");
                }
                else
                {
                    ToastHelper.SendToast("It has been removed from my favorite drawings.");
                }
            }
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
