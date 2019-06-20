using HHChaosToolkit.UWP.Mvvm;
using HHChaosToolkit.UWP.Picker;
using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

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
                        this.Exit();
                        NavigationServiceList.Instance[ShellViewModel.ContentNavigationServiceKey].Navigate(typeof(AnimDrawingViewModel).FullName, result.Result, new DrillInNavigationTransitionInfo());
                    }
                });
            }
        }
    }
}
