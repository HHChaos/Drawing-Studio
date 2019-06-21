using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading.Tasks;

namespace LearnDraw.Services
{
    public static class FirstRunDisplayService
    {
        private static bool shown = false;
        public static bool IsFirstRun => SystemInformation.IsFirstRun;
        internal static async Task ShowIfAppropriateAsync()
        {
            if (IsFirstRun && !shown)
            {
                shown = true;
                await Task.Delay(500);
                await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(UnpackResViewModel).FullName);
                await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(GuideVideoViewModel).FullName);
            }
        }
    }
}
