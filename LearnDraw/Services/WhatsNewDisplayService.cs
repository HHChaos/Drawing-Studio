using Microsoft.Toolkit.Uwp.Helpers;
using System.Threading.Tasks;

namespace LearnDraw.Services
{
    public static class WhatsNewDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync()
        {
            if (SystemInformation.IsAppUpdated && !shown)
            {
                shown = true;
                await Task.CompletedTask;
            }
        }
    }
}
