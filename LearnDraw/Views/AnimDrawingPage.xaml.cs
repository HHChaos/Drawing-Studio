using SvgConverter.SvgParse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        public AnimDrawingPage()
        {
            this.InitializeComponent();
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

            if(e.Parameter is SvgElement svg)
            {
                SvgPlayer.Svg = svg;
            }

        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardInkPanelConnectedAnimation", InkPanel);
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
