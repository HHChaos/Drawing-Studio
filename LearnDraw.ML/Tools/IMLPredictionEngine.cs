using System.IO;

namespace LearnDraw.ML.Tools
{
    public interface IMLPredictionEngine<TSrc, TDst>
    {
        bool BuildPredictionEngine(Stream mlModelStream);
        bool BuildPredictionEngine(string mlModelPath);
        TDst Predict(TSrc data);
        TDst Predict(float[] data);
    }
}
