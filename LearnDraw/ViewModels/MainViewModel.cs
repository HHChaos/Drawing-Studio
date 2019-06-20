using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Core.Models;
using LearnDraw.Helpers;
using LearnDraw.MLHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Input.Inking;

namespace LearnDraw.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        { }

        private string[] _candidateLabels;
        private List<ArtDrawing> _recommendedAssets;

        public List<ArtDrawing> RecommendedAssets
        {
            get => _recommendedAssets;
            set => Set(ref _recommendedAssets, value);
        }

        public string Prediction => _candidateLabels?[0];
        public string CandidateLabel1 => _candidateLabels?[1];
        public string CandidateLabel2 => _candidateLabels?[2];
        public string CandidateLabel3 => _candidateLabels?[3];

        public async Task UpdataPrediction(IEnumerable<InkStroke> strokes)
        {
            var predictionResult = await MLHelper.Instance.Predict(strokes);
            if (predictionResult?.Length > 0)
            {
                _candidateLabels = predictionResult;
                RaisePropertyChanged(() => Prediction);
                RaisePropertyChanged(() => CandidateLabel1);
                RaisePropertyChanged(() => CandidateLabel2);
                RaisePropertyChanged(() => CandidateLabel3);
                RecommendedAssets = AnimAssetsHelper.Instance.GetRecommendedAssets(_candidateLabels);
            }
            else
            {
                ToastHelper.SendToast("The prediction engine has not been loaded yet, please wait...", TimeSpan.FromSeconds(3));
            }
        }

    }
}
