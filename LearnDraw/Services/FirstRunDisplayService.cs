using System;
using System.Threading.Tasks;
using LearnDraw.ViewModels;
using LearnDraw.ViewModels.PickerViewModels;
using LearnDraw.Views;

using Microsoft.Toolkit.Uwp.Helpers;

namespace LearnDraw.Services
{
    public static class FirstRunDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsFirstRun && !shown)
            {
                shown = true;
                await ViewModelLocator.Current.ObjectPickerService.PickSingleObjectAsync<bool>(typeof(FirstRunViewModel).FullName);
            }
        }
    }
}
