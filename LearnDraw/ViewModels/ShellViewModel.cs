using HHChaosToolkit.UWP.Mvvm;
using HHChaosToolkit.UWP.Picker;
using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.ViewModels.PickerViewModels;
using System;
using System.Windows.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LearnDraw.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        public static readonly string ContentNavigationServiceKey = "HomeShellViewModel_Content";

        public ShellViewModel()
        {
        }

        public bool IsShowWelcomeScreen
        {
            get => ApplicationData.Current.LocalSettings.ReadBool(SettingsContract.IsShowWelcomeScreen);
            set
            {
                ApplicationData.Current.LocalSettings.SaveBool(SettingsContract.IsShowWelcomeScreen, value);
                RaisePropertyChanged(() => IsShowWelcomeScreen);
            }
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
        public ICommand OpenMyLibraryCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var result = await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<ArtDrawing>(typeof(MyFavoriteDrawingsViewModel).FullName, null,
                               new PickerOpenOption
                               {
                                   EnableTapBlackAreaExit = true,
                                   VerticalAlignment = VerticalAlignment.Stretch,
                                   HorizontalAlignment = HorizontalAlignment.Stretch,
                                   Background = new SolidColorBrush(Colors.Transparent),
                                   Transitions = new TransitionCollection
                                   {
                                       new EdgeUIThemeTransition{Edge = EdgeTransitionLocation.Top}
                                   }
                               });
                    if (!result.Canceled)
                    {
                        var currentPageType = (NavigationServiceList.Instance[ContentNavigationServiceKey].Frame?.Content as FrameworkElement)?.DataContext?.GetType();
                        if (currentPageType == typeof(AnimDrawingViewModel))
                        {
                            ViewModelLocator.Current.AnimDrawingViewModel.UpdateCurrentArtDrawingChanged(result.Result);
                        }
                        else
                        {
                            NavigationServiceList.Instance[ContentNavigationServiceKey].Navigate(typeof(AnimDrawingViewModel).FullName, result.Result, new DrillInNavigationTransitionInfo());
                        }
                    }
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
