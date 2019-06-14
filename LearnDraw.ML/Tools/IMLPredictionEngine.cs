using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LearnDraw.ML.Tools
{
    public interface IMLPredictionEngine<TSrc, TDst>
    {
        bool BuildPredictionEngine(Stream mlModelData);
        TDst Predict(TSrc data);
        TDst Predict(float[] data);
    }
}
