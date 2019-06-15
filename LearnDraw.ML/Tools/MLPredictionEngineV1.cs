using LearnDraw.ML.Models;
using Microsoft.ML;
using System.IO;

namespace LearnDraw.ML.Tools
{
    public class MLPredictionEngineV1 : IMLPredictionEngine<ModelInputV1, ModelOutputV1>
    {
        MLContext mlContext;
        PredictionEngine<ModelInputV1, ModelOutputV1> predEngine;
        public bool BuildPredictionEngine(Stream mlModelData)
        {
            mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(mlModelData, out DataViewSchema inputSchema);
            predEngine = mlContext.Model.CreatePredictionEngine<ModelInputV1, ModelOutputV1>(mlModel);
            return true;
        }

        public ModelOutputV1 Predict(ModelInputV1 data)
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

        public ModelOutputV1 Predict(float[] data)
        {
            return Predict(new ModelInputV1 { Data = data });
        }
    }
}
