using Microsoft.ML.Data;

namespace LearnDraw.ML.Models
{
    public class ModelInputV1
    {
        [ColumnName("Label"), LoadColumn(0)]
        public string Label { get; set; }
        [ColumnName("Data"), VectorType(784), LoadColumn(1, 784)]
        public float[] Data { get; set; }
    }
}
