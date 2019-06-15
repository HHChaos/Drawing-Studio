using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Input.Inking;

namespace LearnDraw.MLHelpers
{
    public struct PathInfo
    {
        public float Length { get; set; }
        public int PickPointCount { get; set; }
        public CanvasGeometry Path { get; set; }
    }
    public class DataV2ConvertHelper
    {
        public static float[] GetPointArray(IEnumerable<InkStroke> strokes)
        {
            var device = CanvasDevice.GetSharedDevice();
            var pointCount = 150;
            var geometry = CanvasGeometry.CreateInk(device, strokes);
            var bounds = geometry.ComputeBounds();
            var transform = Matrix3x2.CreateTranslation(new Vector2(-(float)bounds.X, -(float)bounds.Y)) * Matrix3x2.CreateScale((float)(255 / Math.Max(bounds.Width, bounds.Height)));
            geometry.Dispose();
            var paths = GetPaths(device, strokes, pointCount, transform);
            var ret = new float[pointCount * 2];

            var points = new List<Vector2>();
            for (int i = 0; i < paths.Length; i++)
            {
                var pathPoints = GetPathPoints(paths[i]);
                if (pathPoints?.Length > 0)
                    points.AddRange(pathPoints);
            }
            for (int i = 0; i < pointCount; i++)
            {
                var index = i > (points.Count - 1) ? points.Count - 1 : i;
                var point = points[index];
                ret[i * 2] = point.X;
                ret[i * 2 + 1] = point.Y;
            }
            return ret;
        }
        private static PathInfo[] GetPaths(ICanvasResourceCreator resourceCreator, IEnumerable<InkStroke> strokes, int totalPickPointCount, Matrix3x2 transform)
        {
            var count = strokes.Count();
            var paths = new PathInfo[count];
            var index = 0;
            foreach (var stroke in strokes)
            {
                var path = CanvasGeometry.CreateInk(resourceCreator, new InkStroke[] { stroke }, transform, 1f);
                paths[index] = new PathInfo
                {
                    Path = path,
                    Length = path.ComputePathLength()
                };
                index++;
            }
            var totalLength = paths.Aggregate<PathInfo, double>(0, (current, item) => current + item.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i].Length > 0)
                    paths[i].PickPointCount = (int)Math.Round(paths[i].Length / totalLength * totalPickPointCount);
            }
            return paths;
        }

        private static Vector2[] GetPathPoints(PathInfo info)
        {
            if (info.PickPointCount < 1)
                return null;
            var points = new Vector2[info.PickPointCount];
            var disGap = info.Length / info.PickPointCount;
            var halfGap = disGap / 2;
            for (int i = 0; i < info.PickPointCount; i++)
            {
                points[i] = info.Path.ComputePointOnPath(disGap * i + halfGap);
            }
            info.Path.Dispose();
            return points;
        }
    }
}
