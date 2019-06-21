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
using Microsoft.Toolkit.Uwp.UI.Animations;
using System.Threading.Tasks;

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

        private void DrawingsGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (DrawingsGridView.SelectionMode != ListViewSelectionMode.Multiple)
            {
                ViewModel.SetResult(e.ClickedItem as ArtDrawing);
            }
        }

        private void MultipleSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            DrawingsGridView.SelectionMode = ListViewSelectionMode.Multiple;
        }

        private void SingleSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            DrawingsGridView.SelectionMode = ListViewSelectionMode.Single;
        }

        private async void MultipleSelectBtn_Loaded(object sender, RoutedEventArgs e)
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
            Implicit.SetShowAnimations(MultipleSelectBtn, showAnimations);
        }
    }
}
