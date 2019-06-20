using LearnDraw.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LearnDraw.Helpers
{
    public class MyFavoriteAssetsHelper
    {
        private MyFavoriteAssetsHelper() { }
        private static MyFavoriteAssetsHelper _instance;
        public static MyFavoriteAssetsHelper Instance => _instance ?? (_instance = new MyFavoriteAssetsHelper());

        private const string SettingsKey = SettingsContract.MyFavoriteAssets;
        private List<ArtDrawing> _myFavoriteAssets;
        private bool _isChanged = false;
        public ReadOnlyCollection<ArtDrawing> MyFavoriteAssets => _myFavoriteAssets?.AsReadOnly();
        public async Task Init()
        {
            _myFavoriteAssets = await ApplicationData.Current.LocalFolder.ReadAsync<List<ArtDrawing>>(SettingsKey);
            if (_myFavoriteAssets == null)
            {
                _myFavoriteAssets = new List<ArtDrawing>();
            }
        }

        public bool AddDrawing(ArtDrawing artDrawing)
        {
            if (_myFavoriteAssets != null)
            {
                _myFavoriteAssets.Add(artDrawing);
                _isChanged = true;
                return true;
            }
            return false;
        }

        public bool Contains(ArtDrawing artDrawing)
        {
            if (_myFavoriteAssets != null)
            {
                if (_myFavoriteAssets.Count == 0 || artDrawing?.FilePath == null)
                    return false;
                var find = _myFavoriteAssets.FirstOrDefault(o => o.FilePath == artDrawing?.FilePath);
                return find != null;
            }
            return false;
        }

        public bool RemoveDrawing(ArtDrawing artDrawing)
        {
            if (_myFavoriteAssets != null)
            {
                var find = _myFavoriteAssets.FirstOrDefault(o => o.FilePath == artDrawing?.FilePath);
                if (find != null)
                {
                    _myFavoriteAssets.Remove(find);
                }
                _isChanged = true;
            }
            return true;
        }
        public bool RemoveDrawings(IEnumerable<ArtDrawing> artDrawings)
        {
            foreach (var item in artDrawings)
            {
                RemoveDrawing(item);
            }
            return true;
        }

        public async Task FlushAsync()
        {
            if (_isChanged)
            {
                await ApplicationData.Current.LocalFolder.SaveAsync(SettingsKey, _myFavoriteAssets);
                _isChanged = false;
            }
        }

    }
}
