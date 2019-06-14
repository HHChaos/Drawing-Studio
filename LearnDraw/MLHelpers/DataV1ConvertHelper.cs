using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Input.Inking;

namespace LearnDraw.MLHelpers
{
    public class DataV1ConvertHelper
    {
        public static CanvasImageSource PrintImage(float[] data)
        {
            if (data.Length == 784)
            {
                var device = CanvasDevice.GetSharedDevice();
                var startPoint = new Vector2();
                var renderTarget = new CanvasImageSource(device, 280, 280, 96f);

                using (var drawingSession = renderTarget.CreateDrawingSession(Colors.White))
                {
                    for (int j = 0; j < 28; j++)
                    {
                        startPoint.X = 0;
                        for (int k = 0; k < 28; k++)
                        {
                            if (data[j * 28 + k] == 1)
                                drawingSession.FillRectangle(startPoint.X, startPoint.Y, 10, 10, Colors.Black);
                            startPoint.X += 10;
                        }
                        startPoint.Y += 10;
                    }
                }
                return renderTarget;
            }
            return null;
        }
        public static float[] GetDotArray(IEnumerable<InkStroke> strokes)
        {
            var ret = new float[28 * 28];
            var device = CanvasDevice.GetSharedDevice();
            var geometry = CanvasGeometry.CreateInk(device, strokes);
            var bounds = geometry.ComputeBounds();
            var transform = Matrix3x2.CreateTranslation(new Vector2(-(float)bounds.X, -(float)bounds.Y)) * Matrix3x2.CreateScale((float)(255 / Math.Max(bounds.Width, bounds.Height)));
            geometry.Dispose();
            using (var renderTarget = new CanvasRenderTarget(device, 255, 255, 96f))
            {

                using (var drawingSession = renderTarget.CreateDrawingSession())
                {
                    drawingSession.Transform = transform;
                    drawingSession.Clear(Colors.Transparent);
                    drawingSession.DrawInk(strokes);
                }
                var dots = GetDotArray(renderTarget, 28, 28);
                for (int j = 0; j < 28; j++)
                {
                    for (int k = 0; k < 28; k++)
                    {
                        ret[j * 28 + k] = dots[j, k] ? 1 : 0;
                    }
                }
            }
            return ret;
        }
        public static bool[,] GetDotArray(CanvasBitmap image, int rowCount, int colCount)
        {
            var path = new bool[rowCount, colCount];
            var startPoint = new Vector2();

            var rectSize = new Vector2((float)image.Bounds.Width / colCount, (float)image.Bounds.Height / rowCount);
            for (var i = 0; i < rowCount; i++)
            {
                startPoint.X = 0;
                for (var j = 0; j < colCount; j++)
                {
                    var pixelColor = image.GetPixelColors((int)(startPoint.X + rectSize.X / 2), (int)(startPoint.Y + rectSize.Y / 2), 1, 1).Single();
                    path[i, j] = pixelColor.A > 0;
                    startPoint.X += rectSize.X;
                }

                startPoint.Y += rectSize.Y;
            }
            return path;
        }
    }
}
