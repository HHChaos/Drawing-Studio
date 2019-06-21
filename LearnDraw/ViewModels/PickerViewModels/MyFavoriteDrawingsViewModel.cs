using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnDraw.ViewModels.PickerViewModels
{
    public class MyFavoriteDrawingsViewModel : ObjectPickerBase<ArtDrawing>
    {
        public ICommand DeleteDrawingsCommand
        {
            get
            {
                return new RelayCommand<IEnumerable<object>>(async list =>
                {
                    var delList = list.Cast<ArtDrawing>();
                    MyFavoriteAssetsHelper.Instance.RemoveDrawings(delList);
                    RaisePropertyChanged(() => MyFavoriteDrawings);
                    await MyFavoriteAssetsHelper.Instance.FlushAsync();
                });
            }
        }
        public ReadOnlyCollection<ArtDrawing> MyFavoriteDrawings => MyFavoriteAssetsHelper.Instance.MyFavoriteAssets;
    }
}
