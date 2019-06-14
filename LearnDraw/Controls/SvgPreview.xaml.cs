using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SvgConverter.SvgParse;
using SvgConverter.SvgParseForWin2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace LearnDraw.Controls
{
    public sealed partial class SvgPreview : UserControl
    {
        public SvgPreview()
        {
            this.InitializeComponent();
        }

        public SvgElement Svg { get; private set; }

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
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(filePath));
            var svgStr = await FileIO.ReadTextAsync(file);
            Svg = SvgElement.LoadFromXml(svgStr);
            SvgImage.Source = await SvgToImageSource(Svg);
        }

        private async Task<CanvasImageSource> SvgToImageSource(SvgElement svg)
        {
            var device = CanvasDevice.GetSharedDevice();
            using (var win2DSvg = await Win2DSvgElement.Parse(device, svg))
            {
                win2DSvg.Progress = 1;
                var offScreen = new CanvasImageSource(device, 200, 200, 96);
                using (var drawingSession = offScreen.CreateDrawingSession(Colors.White))
                {
                    drawingSession.Transform = Matrix3x2.CreateTranslation(new Vector2(-(float)svg.ViewBox.X, -(float)svg.ViewBox.Y)) * Matrix3x2.CreateScale((float)(200 / Math.Max(svg.ViewBox.Width, svg.ViewBox.Height)));
                    win2DSvg.Draw(drawingSession, 0);
                }
                return offScreen;
            }
        }
    }
}
