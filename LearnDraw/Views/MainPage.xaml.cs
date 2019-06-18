using HHChaosToolkit.UWP.Services.Navigation;
using LearnDraw.Controls;
using LearnDraw.ViewModels;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.Views
{

    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MainViewModel; }
        }


        public MainPage()
        {
            InitializeComponent();
            this.DataContext = ViewModel;
            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var compositor = Window.Current.Compositor;
            var rotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            var linear = compositor.CreateLinearEasingFunction();
            rotationAnimation.InsertKeyFrame(1.0f, 360, linear);
            rotationAnimation.Duration = TimeSpan.FromSeconds(9);
            rotationAnimation.Target = nameof(Rectangle1.Rotation);
            rotationAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            Rectangle1.CenterPoint = new Vector3((float)Rectangle1.Width / 2, (float)Rectangle1.Height / 2, 0);
            Rectangle2.CenterPoint = new Vector3((float)Rectangle2.Width / 2, (float)Rectangle2.Height / 2, 0);

            Rectangle1.StartAnimation(rotationAnimation);

            rotationAnimation.Duration = TimeSpan.FromSeconds(8);

            Rectangle2.StartAnimation(rotationAnimation);

            InkCanvas.StrokesChanged += InkCanvas_StrokesChanged;
        }

        private async void InkCanvas_StrokesChanged(Windows.UI.Input.Inking.InkPresenter sender, EventArgs args)
        {
            var strokes = sender.StrokeContainer.GetStrokes();
            if (strokes?.Count > 0)
            {
                HidePredictionUIElement();
                ViewModel.UpdataPrediction(strokes);
                await Task.Delay(300);
                ShowPredictionUIElement();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var inkPanelAnim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardInkPanelConnectedAnimation");
            if (inkPanelAnim != null)
            {
                inkPanelAnim.TryStart(InkPanel);
            }

        }

        private void ThumbnailButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var radioBtn = sender as ContentControl;
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardAnimPanelConnectedAnimation", radioBtn);
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardInkPanelConnectedAnimation", InkPanel);
            NavigationServiceList.Instance[ShellViewModel.ContentNavigationServiceKey].Navigate(typeof(AnimDrawingViewModel).FullName, (radioBtn.Content as SvgPreview)?.Svg);
        }
        private void HidePredictionUIElement()
        {
            TbPrediction.Visibility = Visibility.Collapsed;
            TbCandidate.Visibility = Visibility.Collapsed;
        }
        private void ShowPredictionUIElement()
        {
            TbPrediction.Visibility = Visibility.Visible;
            TbCandidate.Visibility = Visibility.Visible;
        }

    }
}
