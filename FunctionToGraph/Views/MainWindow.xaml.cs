﻿using System;
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
        private const string AlreadyAddedToListMessage = "Already added";
        private const ScatterPlot.NanBehavior OnNanBehaviour = ScatterPlot.NanBehavior.Ignore;
        private readonly AxisLimits _defaultPlotViewport = new AxisLimits(-10.0, 10.0, -10.0, 10.0);
        
        private readonly ExpressionModel _expressionModel;
        private Color _graphColor;
        
        private ObservableCollection<GraphModel> _fixedGraphModels;
        
        private readonly StorageService _storageService;
        private readonly ResourceModel _resourceModel;
        
        public class ResourceModel
        {
            private const string ExpressionModelKey = "ExpressionModel";
            private const string GraphColorKey = "GraphColor";
            private readonly ResourceDictionary _resourceDictionary;
            
            public ExpressionModel GraphExpressionModel => (ExpressionModel)_resourceDictionary[ExpressionModelKey];
            
            public Color GraphColor
            {
                get => (Color)_resourceDictionary[GraphColorKey];
                set
                {
                    _resourceDictionary[GraphColorKey] = value; 
                    OnGraphColorChanged?.Invoke(value);
                }
            }
            
            public event Action<Color> OnGraphColorChanged; 
            
            public ResourceModel(ResourceDictionary resourceDictionary)
            {
                _resourceDictionary = resourceDictionary;
            }
        }


        public MainWindow(StorageService storageService)
        {
            _storageService = storageService;
            _resourceModel = new ResourceModel(Resources);
            
            InitializeComponent();
            
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");
            _plot.Plot.SetAxisLimits(_defaultPlotViewport);
            
            _expressionModel = (ExpressionModel)Resources["ExpressionModel"];
            _graphColor = (Color)Resources["GraphColor"];
            
            _storageService.ReadGraphModelsAsync().ContinueWith(task =>
            {
                _fixedGraphModels = new ObservableCollection<GraphModel>(task.Result);
                _fixedGraphModels.CollectionChanged += UpdateGraphModelsStorage;
                _graphsListView.ItemsSource = _fixedGraphModels;
                
                RedrawScatterPlot();
                
                _expressionModel.OnValidationCheck += OnExpressionValidationCheck;

            }, TaskScheduler.FromCurrentSynchronizationContext());
         
            Closed += OnWindowClosed;
        }
        
        private void UpdateGraphModelsStorage(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _storageService.SaveGraphModelsAsync(_fixedGraphModels);
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
            
            foreach (GraphModel graphModel in _fixedGraphModels)
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
        
        private void ClearScatterPlot()
        {
            _plot.Plot.Clear();
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
        
        private void OnAddToListButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_expressionModel.IsValidated)
            {
                _messageTextBlock.Text = ExpressionModel.IncorrectExpressionMessage;
            }
            else if (IsAlreadyAddedToList(_expressionModel.ExpressionString))
            {
                _messageTextBlock.Text = AlreadyAddedToListMessage;
            }
            else
            {
                _fixedGraphModels.Add(new GraphModel(
                    _expressionModel.ExpressionString, 
                    _expressionModel.XValues, 
                    _expressionModel.YValues, 
                    _graphColor.ToDotNetColor()));
            }
        }

        private bool IsAlreadyAddedToList(string expressionString)
        {
            return _fixedGraphModels.FirstOrDefault(model => model.Expression == expressionString) != default;
        }

        private void OnRemoveFromListButtonClick(object sender, RoutedEventArgs e)
        {
            List<GraphModel> selectedItems = new List<GraphModel>(_graphsListView.SelectedItems.Cast<GraphModel>());

            foreach (GraphModel graphModel in selectedItems)
            {
                _fixedGraphModels.Remove(graphModel);
            }
            
            RedrawScatterPlot();
        }
        
        private void OnGraphColorButtonClick(object sender, RoutedEventArgs e)
        {
            ColorPickerWindow colorPickerWindow = new ColorPickerWindow();
            colorPickerWindow.Show();
            colorPickerWindow.OnColorPicked += OnGraphColorPicked;
        }

        private void OnGraphColorPicked(Color pickedColor)
        {
            _graphColor = pickedColor;
            Resources["GraphColor"] = _graphColor; 
            RedrawScatterPlot();
        }

        private void OnWindowClosed(object sender, EventArgs args)
        {
            _expressionModel.OnValidationCheck -= OnExpressionValidationCheck;
            Closed -= OnWindowClosed;
        }

        
    }
}