using LearnDraw.ML.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnDraw.ML.Tools
{
    public static class MLLabelHelper
    {
        private static readonly Dictionary<int, string> KeyLabels = new Dictionary<int, string>
        {
            [0] = "apple",
            [1] = "baseball",
            [2] = "bicycle",
            [3] = "bird",
            [4] = "book",
            [5] = "bus",
            [6] = "butterfly",
            [7] = "cake",
            [8] = "calculator",
            [9] = "camera",
            [10] = "car",
            [11] = "cat",
            [12] = "circle",
            [13] = "clock",
            [14] = "cloud",
            [15] = "coffee cup",
            [16] = "cup",
            [17] = "eye",
            [18] = "eyeglasses",
            [19] = "face",
            [20] = "flower",
            [21] = "sun",
            [22] = "umbrella",
        };
        public static string GetLabel(int key)
        {
            if (KeyLabels.ContainsKey(key))
            {
                return KeyLabels[key];
            }
            return null;
        }

        public static string[] GetCandidateLabels(this ModelOutputV2 output,int count)
        {
            var labels = new string[count];
            var scores = output.Score.ToList();
            var minValue = scores.Min();
            for (int i = 0; i < count; i++)
            {
                var maxValue = scores.Max();
                var index = scores.IndexOf(maxValue);
                labels[i] = GetLabel(index);
                scores[index] = minValue - 1;
            }
            return labels;
        }
    }
}
