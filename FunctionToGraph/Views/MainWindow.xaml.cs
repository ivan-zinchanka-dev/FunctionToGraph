using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FunctionToGraph.Extensions;
using FunctionToGraph.Models;
using FunctionToGraph.Services;
using ScottPlot;
using ScottPlot.Plottable;
using Color = System.Windows.Media.Color;

namespace FunctionToGraph.Views
{
    public partial class MainWindow : Window
    {
        private const ScatterPlot.NanBehavior OnNanBehaviour = ScatterPlot.NanBehavior.Ignore;
        private static readonly AxisLimits DefaultPlotViewport = new AxisLimits(-10.0, 10.0, -10.0, 10.0);
        
        private readonly ExpressionModel _expressionModel;
        private Color _graphColor;
        private readonly string _alreadyPinnedMessage;
        
        private readonly StorageService _storageService;
        private ObservableCollection<GraphModel> _pinnedGraphModels;
        
        private static class ResourceKeys
        {
            public const string ExpressionModel = "ExpressionModel";
            public const string GraphColor = "GraphColor";
            public const string AlreadyPinnedMessageKey = "AlreadyPinnedMessage";
        }
        
        public MainWindow(StorageService storageService)
        {
            _storageService = storageService;
            
            InitializeComponent();
            
            _expressionModel = (ExpressionModel)Resources[ResourceKeys.ExpressionModel];
            _graphColor = (Color)Resources[ResourceKeys.GraphColor];
            _alreadyPinnedMessage = (string)Resources[ResourceKeys.AlreadyPinnedMessageKey];
            
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");
            _plot.Plot.SetAxisLimits(DefaultPlotViewport);
            
            _storageService.ReadGraphModelsAsync().ContinueWith(task =>
            {
                _pinnedGraphModels = new ObservableCollection<GraphModel>(task.Result);
                _pinnedGraphModels.CollectionChanged += UpdateGraphModelsStorage;
                _graphsListView.ItemsSource = _pinnedGraphModels;
                
                RedrawScatterPlot();
                
                _expressionModel.OnValidationCheck += OnExpressionValidationCheck;

            }, TaskScheduler.FromCurrentSynchronizationContext());
         
            Closed += OnWindowClosed;
        }
        
        private void UpdateGraphModelsStorage(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _storageService.SaveGraphModelsAsync(_pinnedGraphModels);
        }
        
        private void RedrawScatterPlot()
        {
            _plot.Plot.Clear();

            if (_expressionModel.IsValidated)
            {
                _plot.Plot.AddScatter(
                        _expressionModel.XValues, 
                        _expressionModel.YValues, 
                        _graphColor.ToDotNetColor(), 
                        2.0f, 
                        0.0f, 
                        MarkerShape.none, 
                        LineStyle.Solid, 
                        _expressionModel.FullExpressionString)
                    .OnNaN = OnNanBehaviour;
            }
            
            foreach (GraphModel graphModel in _pinnedGraphModels)
            {
                _plot.Plot.AddScatter(
                        graphModel.XValues, 
                        graphModel.YValues, 
                        graphModel.Color, 
                        2.0f, 
                        0.0f, 
                        MarkerShape.none, 
                        LineStyle.Solid, 
                        graphModel.FullExpression)
                    .OnNaN = OnNanBehaviour;
            }

            Settings settings = _plot.Plot.GetSettings();
            settings.EqualScaleMode = EqualScaleMode.PreserveSmallest;
            settings.EnforceEqualAxisScales();
            settings.EqualScaleMode = EqualScaleMode.Disabled;
            
            _plot.Plot.Legend();
            _plot.Refresh();
        }

        private void OnExpressionValidationCheck(ExpressionModel expressionModel, bool validationResult)
        {
            _messageTextBlock.Text = string.Empty;
            
            if (validationResult)
            {
                RedrawScatterPlot();
            }
        }
        
        private void OnPinButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_expressionModel.IsValidated)
            {
                _messageTextBlock.Text = ExpressionModel.IncorrectExpressionMessage;
            }
            else if (IsAlreadyAddedToList(_expressionModel.ExpressionString))
            {
                _messageTextBlock.Text = _alreadyPinnedMessage;
            }
            else
            {
                _pinnedGraphModels.Add(new GraphModel(
                    _expressionModel.ExpressionString, 
                    _expressionModel.XValues, 
                    _expressionModel.YValues, 
                    _graphColor.ToDotNetColor()));
            }
        }

        private bool IsAlreadyAddedToList(string expressionString)
        {
            return _pinnedGraphModels.FirstOrDefault(model => model.Expression == expressionString) != default;
        }

        private void OnUnpinButtonClick(object sender, RoutedEventArgs e)
        {
            List<GraphModel> selectedItems = new List<GraphModel>(_graphsListView.SelectedItems.Cast<GraphModel>());

            foreach (GraphModel graphModel in selectedItems)
            {
                _pinnedGraphModels.Remove(graphModel);
            }
            
            RedrawScatterPlot();
        }
        
        private void OnPickColorButtonClick(object sender, RoutedEventArgs e)
        {
            ColorPickerWindow colorPickerWindow = new ColorPickerWindow(_graphColor);
            colorPickerWindow.Show();
            colorPickerWindow.OnColorPicked += OnGraphColorPicked;
        }

        private void OnGraphColorPicked(Color pickedColor)
        {
            _graphColor = pickedColor;
            Resources[ResourceKeys.GraphColor] = _graphColor; 
            RedrawScatterPlot();
        }

        private void OnWindowClosed(object sender, EventArgs args)
        {
            _expressionModel.OnValidationCheck -= OnExpressionValidationCheck;
            Closed -= OnWindowClosed;
        }
    }
}