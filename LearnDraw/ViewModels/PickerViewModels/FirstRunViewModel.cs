using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Helpers;
using System;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class FirstRunViewModel : ObjectPickerBase<bool>
    {
        public override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await DecompressionAssets();
            this.Exit();
        }

        private async Task DecompressionAssets()
        {
            try
            {
                var assetsFoloder = await ApplicationData.Current.LocalFolder.GetFolderAsync("AnimAssets");
                if (assetsFoloder != null)
                    await assetsFoloder.DeleteAsync();
            }
            catch (Exception)
            {
            }
            var assets = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Zip/AnimAssets.zip"));
            await Task.Run(() =>
            {
                ZipFile.ExtractToDirectory(assets.Path, ApplicationData.Current.LocalFolder.Path);
            });
            //var assetsFoloder = await ApplicationData.Current.LocalFolder.GetFolderAsync("AnimAssets");
            //if (assetsFoloder != null)
            //{
            //    var folders = await assetsFoloder.GetFoldersAsync();
            //    var animAssets = new AnimAssets { Data = new Dictionary<string, string[]>() };
            //    foreach (var folder in folders)
            //    {
            //        var key = folder.DisplayName.Replace('-', ' ');
            //        var urlList = new List<string>();
            //        var files = await folder.GetFilesAsync();
            //        foreach (var file in files)
            //        {
            //            if (file.Name.EndsWith(".svg"))
            //            {
            //                urlList.Add($"ms-appdata:///local/AnimAssets/{folder.DisplayName}/{file.Name}");
            //            }
            //        }
            //        if (urlList.Count > 0)
            //        {
            //            animAssets.Data[key] = urlList.ToArray();
            //        }
            //    }
            //    var jsonStr = await Json.StringifyAsync(animAssets);
            //    var listFile = await assetsFoloder.CreateFileAsync("assetList.json", CreationCollisionOption.ReplaceExisting);
            //    await FileIO.WriteTextAsync(listFile, jsonStr);
            //}
            var assetsListFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appdata:///local/AnimAssets/assetList.json"));
            await AnimAssetsHelper.Instance.Init(assetsListFile);

        }

    }
}
