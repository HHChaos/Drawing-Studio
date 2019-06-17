using HHChaosToolkit.UWP.Mvvm;
using LearnDraw.Helpers;
using LearnDraw.ML.Tools;
using LearnDraw.MLHelpers;
using System;
using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace LearnDraw.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        { }

        private string _prediction;
        private string[] _candidateLabels;

        public string Prediction
        {
            get => _prediction;
            set => Set(ref _prediction, value);
        }

        private List<string> _recommendedAssets;

        public List<string> RecommendedAssets
        {
            get => _recommendedAssets;
            set => Set(ref _recommendedAssets, value);
        }

        public string CandidateLabel1 => _candidateLabels?[1];
        public string CandidateLabel2 => _candidateLabels?[2];
        public string CandidateLabel3 => _candidateLabels?[3];

        public void UpdataPrediction(IEnumerable<InkStroke> strokes)
        {
            var predictionResult = MLHelper.Instance.Predict(strokes);
            if (predictionResult != null)
            {
                Prediction = predictionResult.Prediction;
                _candidateLabels = predictionResult.GetCandidateLabels(4);
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
