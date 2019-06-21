using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LearnDraw.Views.Pickers
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GuideVideoPage : Page
    {
        private GuideVideoViewModel ViewModel
        {
            get { return ViewModelLocator.Current.GuideVideoViewModel; }
        }
        public GuideVideoPage()
        {
            RequestedTheme = ElementTheme.Dark;
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
