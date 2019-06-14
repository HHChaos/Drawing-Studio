using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnDraw.Core.Models
{
    public class AnimAssets
    {
        [JsonProperty("data")]
        public Dictionary<string, string[]> Data { get; set; }
    }
}
