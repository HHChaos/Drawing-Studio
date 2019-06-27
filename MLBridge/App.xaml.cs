using LearnDraw.Core.Models;
using LearnDraw.ML.Tools;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace MLBridge
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private readonly MLPredictionEngineV2 mLPredictionEngineV2 = new MLPredictionEngineV2();
        public App()
        {
            InitializeAppServiceConnection();
        }
        private AppServiceConnection connection = null;
        private bool _inited;
        private async void InitializeAppServiceConnection()
        {
            try
            {
                connection = new AppServiceConnection
                {
                    AppServiceName = "MLBridgeService",
                    PackageFamilyName = Package.Current.Id.FamilyName
                };
                connection.RequestReceived += Connection_RequestReceived;
                connection.ServiceClosed += Connection_ServiceClosed;
                var status = await connection.OpenAsync();
                if (status != AppServiceConnectionStatus.Success)
                {
                    await SendMessage(AppServiceContract.Exception, $"AppServiceConnectionStatus : {status.ToString()}");
                    Current.Shutdown();
                }
                var exePath = Assembly.GetExecutingAssembly().Location;
                _inited = mLPredictionEngineV2.BuildPredictionEngine(Path.Combine($"{exePath.Substring(0, exePath.LastIndexOf("\\"))}", @"Model\MLModelV20620.zip"));
                await SendMessage(AppServiceContract.InitedMsg, "The prediction engine has been loaded, Enjoy it!");

            }
            catch (Exception ex)
            {
                await SendMessage(AppServiceContract.Exception, ex.ToString());
                Current.Shutdown();
            }
        }
        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                Current.Shutdown();
            }));
        }
        private async void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            if (!_inited)
            {
                await args.Request.SendResponseAsync(new ValueSet
                {
                    { AppServiceContract.Exception,"Initialization is not complete yet!" }
                });
                return;
            }
            else
            {
                string result = string.Empty;
                if (args.Request.Message.ContainsKey(AppServiceContract.RequestPredict))
                {
                    var dataStr = args.Request.Message[AppServiceContract.RequestPredict]?.ToString();

                    var data = JsonConvert.DeserializeObject<float[]>(dataStr);
                    var prediction = mLPredictionEngineV2.Predict(data);
                    if (prediction != null)
                        result = JsonConvert.SerializeObject(prediction.GetCandidateLabels(4));
                }

                await args.Request.SendResponseAsync(new ValueSet
                {
                    { AppServiceContract.Prediction, result}
                });
            }
        }

        private async Task SendMessage(string key, string message)
        {
            await connection.SendMessageAsync(new ValueSet
                {
                    { key, message }
                });
        }
    }
}
