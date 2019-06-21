using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using SvgConverter.SvgParse;
using SvgConverter.SvgParseForWin2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;

namespace LearnDraw.Helpers
{
    public class SvgElementCacheHelper
    {
        private SvgElementCacheHelper() { }
        private static SvgElementCacheHelper _instance;
        public static SvgElementCacheHelper Instance => _instance ?? (_instance = new SvgElementCacheHelper());
        private readonly Dictionary<string, SvgElement> _cacheSvgDic = new Dictionary<string, SvgElement>();
        private readonly Dictionary<string, CanvasImageSource> _cacheSvgImageSourceDic = new Dictionary<string, CanvasImageSource>();
        private Size _cacheImageResolution = new Size(200, 200);
        public async Task<SvgElement> TryGetSvgElementAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            if (_cacheSvgDic.ContainsKey(filePath))
            {
                return _cacheSvgDic[filePath];
            }
            else
            {
                try
                {
                    var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(filePath));
                    var svgStr = await FileIO.ReadTextAsync(file);
                    var svgElement = SvgElement.LoadFromXml(svgStr);
                    _cacheSvgDic[filePath] = svgElement;
                    return svgElement;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        public async Task<CanvasImageSource> TryGetImageSourceAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;
            if (_cacheSvgImageSourceDic.ContainsKey(filePath))
            {
                return _cacheSvgImageSourceDic[filePath];
            }
            else
            {
                try
                {
                    var svgElement = await TryGetSvgElementAsync(filePath);
                    var imageSource = await SvgToImageSource(svgElement);
                    _cacheSvgImageSourceDic[filePath] = imageSource;
                    return imageSource;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }

        private async Task<CanvasImageSource> SvgToImageSource(SvgElement svg)
        {
            if (svg == null)
                return null;
            var device = CanvasDevice.GetSharedDevice();
            using (var win2DSvg = await Win2DSvgElement.Parse(device, svg))
            {
                win2DSvg.Progress = 1;
                var offScreen = new CanvasImageSource(device, (float)_cacheImageResolution.Width, (float)_cacheImageResolution.Height, 96);
                using (var drawingSession = offScreen.CreateDrawingSession(Colors.Transparent))
                {
                    drawingSession.Transform = Matrix3x2.CreateTranslation(new Vector2(-(float)svg.ViewBox.X, -(float)svg.ViewBox.Y)) * Matrix3x2.CreateScale((float)(200 / Math.Max(svg.ViewBox.Width, svg.ViewBox.Height)));
                    win2DSvg.Draw(drawingSession, 0);
                }
                return offScreen;
            }
        }
    }
}
