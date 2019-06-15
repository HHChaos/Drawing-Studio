using System.IO;

namespace LearnDraw.ML.Tools
{
    public interface IMLPredictionEngine<TSrc, TDst>
    {
        bool BuildPredictionEngine(Stream mlModelData);
        TDst Predict(TSrc data);
        TDst Predict(float[] data);
    }
}
