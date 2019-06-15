using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SvgConverter.SvgParse;
using SvgConverter.SvgParseForWin2D;
using System;
using System.Numerics;
using System.Threading;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace LearnDraw.Controls
{
    public sealed partial class SvgAnimPlayer : UserControl
    {
        private class HandInfo
        {
            public Uri Source { get; set; }
            public Point PenOffect { get; set; }
        }
        private readonly SynchronizationContext _context = SynchronizationContext.Current;
        private ICanvasResourceCreator ResourceCreator { get; } = CanvasDevice.GetSharedDevice();
        private readonly object _lockobj = new object();
        private Win2DSvgElement _win2DSvg;
        private float _drawGap;
        private CanvasBitmap _handBitmap;
        private bool _paused = false;
        private bool _loading = false;
        private float _scale = 1f;
        private Matrix3x2 _scaleMatrix;
        public SvgAnimPlayer()
        {
            this.InitializeComponent();
            if (!DesignMode.DesignModeEnabled)
            {
                Canvas.CreateResources += Canvas_CreateResources;
                SizeChanged += SvgAnimPlayer_SizeChanged;
                Unloaded += SvgAnimPlayer_Unloaded; ;
            }
        }

        public void Play()
        {
            if (_loading || _win2DSvg == null)
                return;
            lock (_lockobj)
            {
                _win2DSvg.Progress = 0;
            }
            _paused = false;
        }
        public void Pause()
        {
            if (_loading || _win2DSvg == null)
                return;
            _paused = true;
        }

        private void SvgAnimPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            lock (_lockobj)
            {
                _win2DSvg?.Dispose();
                _win2DSvg = null;
                _handBitmap?.Dispose();
                _handBitmap = null;
            };
        }


        private readonly HandInfo Hand = new HandInfo
        {
            Source = new Uri("ms-appx:///Assets/Hand.png"),
            PenOffect = new Point(17, 91)
        };
        private void SvgAnimPlayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSvgLayout();
        }
        private async void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            if (Hand != null)
                using (var randomAccessStream = new InMemoryRandomAccessStream())
                {
                    var handFile =
                        await StorageFile.GetFileFromApplicationUriAsync(Hand.Source);
                    var buffer = await FileIO.ReadBufferAsync(handFile);
                    await randomAccessStream.WriteAsync(buffer);
                    randomAccessStream.Seek(0);
                    _handBitmap = await CanvasBitmap.LoadAsync(ResourceCreator, randomAccessStream);
                }
            _drawGap = 1 / 8f * (float)sender.TargetElapsedTime.TotalSeconds;
        }
        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            if (_loading || _win2DSvg == null)
                return;
            args.DrawingSession.Transform = _scaleMatrix;
            var needDrawLenght = _paused ? 0 : _drawGap;
            lock (_lockobj)
            {
                var handPosition = _win2DSvg?.Draw(args.DrawingSession, needDrawLenght);
                if (Hand != null && handPosition != null && _handBitmap != null)
                {
                    var position = handPosition.Value - Hand.PenOffect.ToVector2();
                    args.DrawingSession.Transform = Matrix3x2.Identity;
                    args.DrawingSession.DrawImage(_handBitmap, position);
                }
            }

            _context.Post(_ =>
            {

            }, null);
        }

        public SvgElement Svg
        {
            get { return (SvgElement)GetValue(SvgProperty); }
            set { SetValue(SvgProperty, value); }
        }

        public static readonly DependencyProperty SvgProperty =
            DependencyProperty.Register("Svg", typeof(SvgElement), typeof(SvgAnimPlayer), new PropertyMetadata(null, new PropertyChangedCallback(OnSvgPropertyChanged)));

        private static void OnSvgPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SvgAnimPlayer target)
            {
                target.OnSvgChanged((SvgElement)e.NewValue);
            }
        }

        private async void OnSvgChanged(SvgElement svg)
        {
            var win2DSvg = await Win2DSvgElement.Parse(ResourceCreator, svg);
            _loading = true;
            _paused = true;
            lock (_lockobj)
            {
                _win2DSvg?.Dispose();
                _win2DSvg = win2DSvg;
                UpdateSvgLayout();
                _win2DSvg.Progress = 1;
            }
            _loading = false;
        }
        private void UpdateSvgLayout()
        {
            if (_loading || _win2DSvg == null)
                return;
            var panelWidth = Canvas.ActualWidth - 120;
            var panelHeight = Canvas.ActualHeight - 120;
            var scaleX = panelWidth / _win2DSvg.ViewBox.Width;
            var scaleY = panelHeight / _win2DSvg.ViewBox.Height;
            _scale = (float)Math.Min(scaleX, scaleY);
            _scaleMatrix = Matrix3x2.CreateScale(_scale);
            _scaleMatrix.Translation = new Point(
                    (panelWidth - _win2DSvg.ViewBox.Width * _scale) / 2 - _win2DSvg.ViewBox.X * _scale + 60,
                    (panelHeight - _win2DSvg.ViewBox.Height * _scale) / 2 - _win2DSvg.ViewBox.Y * _scale + 60)
                .ToVector2();
        }
    }
}
