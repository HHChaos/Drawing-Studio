using LearnDraw.ML.Models;
using LearnDraw.ML.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Input.Inking;

namespace LearnDraw.MLHelpers
{
    public class MLHelper
    {
        private MLHelper() { }
        private static MLHelper _instance;
        public static MLHelper Instance => _instance ?? (_instance = new MLHelper());

        private readonly MLPredictionEngineV2 mLPredictionEngineV2 = new MLPredictionEngineV2();
        public async Task<bool> Init(StorageFile mlModel)
        {
            var inited = false;
            using (var fileStream = await mlModel.OpenAsync(FileAccessMode.Read))
            {
                await Task.Run(() =>
                {
                    inited = mLPredictionEngineV2.BuildPredictionEngine(fileStream.AsStream());
                });
            }
            return inited;
        }
        public ModelOutputV2 Predict(IEnumerable<InkStroke> strokes)
        {
            var data = DataV2ConvertHelper.GetPointArray(strokes);
            return mLPredictionEngineV2.Predict(data);
        }

    }
}
