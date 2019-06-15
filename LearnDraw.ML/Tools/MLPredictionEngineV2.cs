using LearnDraw.ML.Models;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LearnDraw.ML.Tools
{
    public class MLPredictionEngineV2 : IMLPredictionEngine<ModelInputV2, ModelOutputV2>
    {
        MLContext mlContext;
        PredictionEngine<ModelInputV2, ModelOutputV2> predEngine;
        public bool BuildPredictionEngine(Stream mlModelData)
        {
            mlContext = new MLContext();
            var mlModel = mlContext.Model.Load(mlModelData, out DataViewSchema inputSchema);
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
