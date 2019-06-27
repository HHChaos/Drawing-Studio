using LearnDraw.Core.Helpers;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace LearnDraw.MLHelpers
{
    public class MLHelper
    {
        private MLHelper() { }
        private static MLHelper _instance;
        public static MLHelper Instance => _instance ?? (_instance = new MLHelper());
        private bool _inited;
        private readonly SynchronizationContext _context = SynchronizationContext.Current;
        public async void Init()
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                App.AppServiceConnected += App_AppServiceConnected;
                App.AppServiceDisconnected += App_AppServiceDisconnected;
                ToastHelper.SendToast("The prediction engine is loading...", TimeSpan.FromSeconds(3));
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            };
        }

        private void App_AppServiceDisconnected(object sender, EventArgs e)
        {
            _inited = false;
            _context?.Post(async _ =>
            {
                ToastHelper.SendToast("Sorry, the prediction engine is down, trying to restart it!", TimeSpan.FromSeconds(3));
                await Task.Delay(500);
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }, null);
        }

        private void App_AppServiceConnected(object sender, AppServiceTriggerDetails e)
        {
            App.Connection.RequestReceived += Connection_RequestReceived;
        }

        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            if (args.Request.Message?.Count > 0)
            {
                foreach (var message in args.Request.Message)
                {
                    switch (message.Key)
                    {
                        case AppServiceContract.InitedMsg:
                            _inited = true;
                            _context?.Post(_ =>
                            {
                                ToastHelper.SendToast(message.Value?.ToString(), TimeSpan.FromSeconds(4));
                            }, null);
                            break;
                        case AppServiceContract.Exception:
                            _context?.Post(_ =>
                            {
                                ToastHelper.SendToast(message.Value?.ToString(), TimeSpan.FromSeconds(6));
                            }, null);
                            break;
                    }
                }
            }
        }

        public async Task<string[]> Predict(IEnumerable<InkStroke> strokes)
        {
            if (!_inited)
                return null;
            var data = DataV2ConvertHelper.GetPointArray(strokes);
            var request = new ValueSet();
            var dataStr = await Json.StringifyAsync(data);
            request.Add(AppServiceContract.RequestPredict, dataStr);
            var response = await App.Connection.SendMessageAsync(request);
            if (response.Status == AppServiceResponseStatus.Success)
            {
                if (response.Message.ContainsKey(AppServiceContract.Prediction))
                {
                    var result = response.Message[AppServiceContract.Prediction].ToString();
                    if (!string.IsNullOrEmpty(result))
                    {
                        return await Json.ToObjectAsync<string[]>(result);
                    }
                }
            }
            return null;
        }
    }
}
