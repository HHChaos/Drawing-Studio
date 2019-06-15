using Newtonsoft.Json;
using System.Collections.Generic;

namespace LearnDraw.Core.Models
{
    public class AnimAssets
    {
        [JsonProperty("data")]
        public Dictionary<string, string[]> Data { get; set; }
    }
}
