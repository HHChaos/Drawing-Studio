using LearnDraw.ML.Models;
using Microsoft.ML;
using System.IO;

namespace LearnDraw.ML.Tools
{
    public class MLPredictionEngineV2 : IMLPredictionEngine<ModelInputV2, ModelOutputV2>
    {
        MLContext mlContext;
        PredictionEngine<ModelInputV2, ModelOutputV2> predEngine;
        public bool BuildPredictionEngine(Stream mlModelStream)
        {
            mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(mlModelStream, out DataViewSchema inputSchema);
            predEngine = mlContext.Model.CreatePredictionEngine<ModelInputV2, ModelOutputV2>(mlModel);
            return true;
        }

        public bool BuildPredictionEngine(string mlModelPath)
        {
            mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(mlModelPath, out DataViewSchema inputSchema);
            predEngine = mlContext.Model.CreatePredictionEngine<ModelInputV2, ModelOutputV2>(mlModel);
            return true;
        }

        public ModelOutputV2 Predict(ModelInputV2 data)
        {
            if (predEngine != null)
            {
                return predEngine.Predict(data);
            }
            else
            {
                return null;
            }
        }

        public ModelOutputV2 Predict(float[] data)
        {
            return Predict(new ModelInputV2 { Data = data });
        }
    }
}
