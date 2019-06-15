using LearnDraw.Core.Helpers;
using LearnDraw.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace LearnDraw.Helpers
{
    public class AnimAssetsHelper
    {
        private AnimAssetsHelper() { }
        private static AnimAssetsHelper _instance;
        public static AnimAssetsHelper Instance => _instance ?? (_instance = new AnimAssetsHelper());

        private AnimAssets assets;
        public async Task<bool> Init(StorageFile assetsListFile)
        {
            string jsonStr = null;
            using (var stream = await assetsListFile.OpenReadAsync())
            using (var streamReader = new StreamReader(stream.AsStream()))
            {
                jsonStr = await streamReader.ReadToEndAsync();
            }
            if (!string.IsNullOrEmpty(jsonStr))
                assets = await Json.ToObjectAsync<AnimAssets>(jsonStr);
            return true;
        }

        public List<string> GetRecommendedAssets(string[] candidateLabels)
        {
            var list = new List<string>();
            foreach (var label in candidateLabels)
            {
                if (assets?.Data?.ContainsKey(label) == true)
                {
                    list.AddRange(assets.Data[label]);
                }
            }
            return list;
        }
    }
}
