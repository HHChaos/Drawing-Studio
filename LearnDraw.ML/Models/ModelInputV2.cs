using Microsoft.ML.Data;

namespace LearnDraw.ML.Models
{
    public class ModelInputV2
    {
        [ColumnName("Label"), LoadColumn(0)]
        public string Label { get; set; }
        [ColumnName("Data"), VectorType(300), LoadColumn(1, 300)]
        public float[] Data { get; set; }
    }
}
