using LearnDraw.Helpers;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SvgConverter.SvgParse;
using SvgConverter.SvgParseForWin2D;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace LearnDraw.Controls
{
    public sealed partial class SvgPreview : UserControl
    {
        public SvgPreview()
        {
            this.InitializeComponent();
            this.Loaded += SvgPreview_Loaded;
        }

        private async void SvgPreview_Loaded(object sender, RoutedEventArgs e)
        {
            if (SvgImage.Source == null && !string.IsNullOrEmpty(FilePath))
            {
                SvgImage.Source = await SvgElementCacheHelper.Instance.TryGetImageSourceAsync(FilePath);
            }
        }
        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FilePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(SvgPreview), new PropertyMetadata(null, new PropertyChangedCallback(OnFilePathPropertyChanged)));

        private static void OnFilePathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SvgPreview target)
            {
                target.OnFilePathChanged((string)e.NewValue);
            }
        }

        private async void OnFilePathChanged(string filePath)
        {
            SvgImage.Source = await SvgElementCacheHelper.Instance.TryGetImageSourceAsync(filePath);
        }
    }
}
