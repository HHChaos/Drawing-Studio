using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using SvgConverter.SvgParse;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LearnDraw.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AnimDrawingPage : Page
    {
        private AnimDrawingViewModel ViewModel
        {
            get { return ViewModelLocator.Current.AnimDrawingViewModel; }
        }

        public AnimDrawingPage()
        {
            this.InitializeComponent();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var animPanelAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardAnimPanelConnectedAnimation");
            var inkPanelAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardInkPanelConnectedAnimation");
            if (animPanelAnim != null)
            {
                animPanelAnim.TryStart(AnimPanel);
            }
            if (inkPanelAnim != null)
            {
                inkPanelAnim.TryStart(InkPanel);
            }

        }

        private async void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardInkPanelConnectedAnimation", InkPanel);
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
            await MyFavoriteAssetsHelper.Instance.FlushAsync();
        }

        private async void PlayBtn_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(1000);
            var showAnimations = new AnimationCollection
            {
                new TranslationAnimation
                {
                    Delay =TimeSpan.FromSeconds(0.3),
                    SetInitialValueBeforeDelay =true,
                    Duration =TimeSpan.FromSeconds(0.6),
                    From ="0,-20,0",
                    To ="0,0,0"
                },
                new OpacityAnimation
                {
                    Delay =TimeSpan.FromSeconds(0.3),
                    SetInitialValueBeforeDelay =true,
                    Duration =TimeSpan.FromSeconds(0.6),
                    From =0,
                    To =1
                }
            };
            Implicit.SetShowAnimations(PlayBtn, showAnimations);
        }
    }
}
