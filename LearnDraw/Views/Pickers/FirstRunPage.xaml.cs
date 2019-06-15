using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace LearnDraw.Views.Pickers
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FirstRunPage : Page
    {
        private FirstRunViewModel ViewModel
        {
            get { return ViewModelLocator.Current.FirstRunViewModel; }
        }
        public FirstRunPage()
        {
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            this.InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
