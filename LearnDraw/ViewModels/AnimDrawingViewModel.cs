using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.ViewModels.PickerViewModels;
using SvgConverter.SvgParse;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.ViewModels
{
    public class AnimDrawingViewModel : ViewModelBase
    {
        public AnimDrawingViewModel()
        {
        }

        private ArtDrawing _currentArtDrawing;
        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is ArtDrawing artDrawing)
            {
                UpdateCurrentArtDrawingChanged(artDrawing);
            }
        }

        public async void UpdateCurrentArtDrawingChanged(ArtDrawing artDrawing)
        {
            _currentArtDrawing = artDrawing;
            RaisePropertyChanged(() => IsFavorite);
            Svg = await SvgElementCacheHelper.Instance.TryGetSvgElementAsync(artDrawing.FilePath);
        }

        private SvgElement _svg;

        public SvgElement Svg
        {
            get => _svg;
            set => Set(ref _svg, value);
        }

        public int Speed
        {
            get => ApplicationData.Current.LocalSettings.ReadInt(SettingsContract.AnimPlaySpeed);
            set
            {
                ApplicationData.Current.LocalSettings.SaveInt(SettingsContract.AnimPlaySpeed, value);
                RaisePropertyChanged(() => Speed);
            }
        }
        public bool IsFavorite
        {
            get => MyFavoriteAssetsHelper.Instance.Contains(_currentArtDrawing);
            set
            {
                if (value == IsFavorite)
                    return;
                if (value)
                {
                    if (MyFavoriteAssetsHelper.Instance.AddDrawing(_currentArtDrawing))
                        ToastHelper.SendFavoriteToast("MyFavoriteDrawingsAddedTip".GetLocalized());
                }
                else
                {
                    if (MyFavoriteAssetsHelper.Instance.RemoveDrawing(_currentArtDrawing))
                        ToastHelper.SendToast("MyFavoriteDrawingsRemovedTip".GetLocalized());
                }
                RaisePropertyChanged(() => IsFavorite);
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
