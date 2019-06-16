using HHChaosToolkit.UWP.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDraw.Helpers
{
    public class ToastHelper
    {
        public static void SendToast(string content, TimeSpan? duration = null)
        {
            var toast = new Toast(content);
            if (duration.HasValue)
            {
                toast.Duration = duration.Value;
            }
            toast.Show();
        }
    }
}
