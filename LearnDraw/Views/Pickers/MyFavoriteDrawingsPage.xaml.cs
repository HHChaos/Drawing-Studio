using LearnDraw.Core.Models;
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
    public sealed partial class MyFavoriteDrawingsPage : Page
    {
        private MyFavoriteDrawingsViewModel ViewModel
        {
            get { return ViewModelLocator.Current.MyFavoriteDrawingsViewModel; }
        }
        public MyFavoriteDrawingsPage()
        {
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SetResult(e.ClickedItem as ArtDrawing);
        }
    }
}
