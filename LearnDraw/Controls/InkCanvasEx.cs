using HHChaosToolkit.UWP.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;

namespace LearnDraw.Controls
{
    public class InkCanvasEx : InkCanvas
    {
        private readonly List<InkStroke> _redoList = new List<InkStroke>();
        private readonly List<InkStroke> _undoList = new List<InkStroke>();
        private RelayCommand _redoCommand;
        private RelayCommand _undoCommand;
        private RelayCommand _saveCommand;

        public InkCanvasEx()
        {
            InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse |
                                                      CoreInputDeviceTypes.Pen |
                                                      CoreInputDeviceTypes.Touch;
            InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        public RelayCommand UndoCommand
        {
            get
            {
                return _undoCommand ?? (_undoCommand = new RelayCommand(() =>
                {
                    if (_redoList.Count > 0)
                    {
                        var item = _redoList.Last();
                        _redoList.Remove(item);
                        _undoList.Add(item.Clone());
                        item.Selected = true;
                        InkPresenter.StrokeContainer.DeleteSelected();
                        StrokesChanged?.Invoke(InkPresenter, EventArgs.Empty);
                    }

                    _undoCommand?.OnCanExecuteChanged();
                    _redoCommand?.OnCanExecuteChanged();
                }, () => _redoList.Count > 0));
            }
        }

        public RelayCommand RedoCommand
        {
            get
            {
                return _redoCommand ?? (_redoCommand = new RelayCommand(() =>
                {
                    if (_undoList.Count > 0)
                    {
                        var item = _undoList.Last();
                        _undoList.Remove(item);
                        InkPresenter.StrokeContainer.AddStroke(item);
                        _redoList.Add(item);
                        StrokesChanged?.Invoke(InkPresenter, EventArgs.Empty);
                    }

                    _undoCommand?.OnCanExecuteChanged();
                    _redoCommand?.OnCanExecuteChanged();
                }, () => _undoList.Count > 0));
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(() =>
                {

                }));
            }
        }

        public event TypedEventHandler<InkPresenter, EventArgs> StrokesChanged;

        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            foreach (var item in args.Strokes)
            {
                var strokeBuilder = new InkStrokeBuilder();
                strokeBuilder.SetDefaultDrawingAttributes(item.DrawingAttributes);
                var stroke = strokeBuilder.CreateStrokeFromInkPoints(item.GetInkPoints(), item.PointTransform);
                _undoList.Add(stroke);
                if (_redoList.Contains(item))
                    _redoList.Remove(item);
            }

            _undoCommand?.OnCanExecuteChanged();
            _redoCommand?.OnCanExecuteChanged();
            StrokesChanged?.Invoke(sender, EventArgs.Empty);
        }

        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            foreach (var item in args.Strokes) _redoList.Add(item);
            _undoList.Clear();
            _undoCommand?.OnCanExecuteChanged();
            _redoCommand?.OnCanExecuteChanged();
            StrokesChanged?.Invoke(sender, EventArgs.Empty);
        }
    }
}
