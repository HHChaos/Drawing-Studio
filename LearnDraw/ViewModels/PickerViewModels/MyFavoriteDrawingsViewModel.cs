using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class MyFavoriteDrawingsViewModel : ObjectPickerBase<ArtDrawing>
    {
        public MyFavoriteDrawingsViewModel()
        {
            this.ObjectPicked += MyFavoriteDrawingsViewModel_ObjectPicked;
            this.Canceled += MyFavoriteDrawingsViewModel_Canceled;
        }

        private async void MyFavoriteDrawingsViewModel_Canceled(object sender, EventArgs e)
        {
            await MyFavoriteAssetsHelper.Instance.FlushAsync(); ;
        }

        private async void MyFavoriteDrawingsViewModel_ObjectPicked(object sender, HHChaosToolkit.UWP.Picker.ObjectPickedEventArgs<ArtDrawing> e)
        {
            await MyFavoriteAssetsHelper.Instance.FlushAsync(); ;
        }

        public ReadOnlyCollection<ArtDrawing> MyFavoriteDrawings => MyFavoriteAssetsHelper.Instance.MyFavoriteAssets;
    }
}
