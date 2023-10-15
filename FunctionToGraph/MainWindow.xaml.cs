using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FunctionToGraph.Extensions;
using FunctionToGraph.Models;
using FunctionToGraph.Resources.Logical;
using FunctionToGraph.Utilities;
using FunctionToGraph.Views;
using ScottPlot;
using Color = System.Windows.Media.Color;

namespace FunctionToGraph
{
    public partial class MainWindow : Window
    {
        private readonly ExpressionModel _expressionModel;
        private ObservableCollection<GraphModel> _fixedGraphModels;

        public MainWindow()
        {
            InitializeComponent();
            
            _plot.Plot.Title("Graph");
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");
            
            _expressionModel = (ExpressionModel)Resources["ExpressionModel"];

            StorageUtility.ReadGraphModelsAsync().ContinueWith(task =>
            {
                _fixedGraphModels = new ObservableCollection<GraphModel>(task.Result);
                _fixedGraphModels.CollectionChanged += UpdateGraphModelsStorage;
                _graphsListView.ItemsSource = _fixedGraphModels;
                
                RedrawScatterPlot();
                
                _expressionModel.OnValidationCheck += OnExpressionValidationCheck;
                AppResources.OnGraphColorCahnged += OnGraphColorChanged;

            }, TaskScheduler.FromCurrentSynchronizationContext());
         
            Closed += OnWindowClosed;
        }
        
        private void UpdateGraphModelsStorage(object? sender, NotifyCollectionChangedEventArgs e)
        {
            StorageUtility.SaveGraphModelsAsync(_fixedGraphModels);
        }
        
        private void RedrawScatterPlot()
        {
            _plot.Plot.Clear();

            if (_expressionModel.IsValidated)
            {
                _plot.Plot.AddScatter(_expressionModel.XValues, _expressionModel.YValues, 
                    AppResources.GraphColor.ToDotNetColor(), 2.0f, 0.0f, MarkerShape.none,
                    LineStyle.Solid, _expressionModel.FullExpressionString);
            }
            
            foreach (GraphModel graphModel in _fixedGraphModels)
            {
                _plot.Plot.AddScatter(graphModel.XValues, graphModel.YValues, 
                    graphModel.Color, 2.0f, 0.0f, MarkerShape.none,
                    LineStyle.Solid, graphModel.FullExpression);
            }
            
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
            if (validationResult)
            {
                RedrawScatterPlot();
            }
        }

        private void OnGraphColorChanged(Color color)
        {
            RedrawScatterPlot();
        }
        
        private void OnAddToListButtonClick(object sender, RoutedEventArgs e)
        {
            if (_expressionModel.IsValidated)
            {
                GraphModel equalModel = _fixedGraphModels.FirstOrDefault(model => model.Expression == _expressionModel.ExpressionString);

                if (equalModel == default)
                {
                    _fixedGraphModels.Add(new GraphModel(_expressionModel.ExpressionString, _expressionModel.XValues, 
                        _expressionModel.YValues, AppResources.GraphColor.ToDotNetColor()));
                }
            }
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
            GraphColorWindow graphColorWindow = new GraphColorWindow();
            graphColorWindow.Show();
        }
        
        private void OnWindowClosed(object? sender, EventArgs args)
        {
            _expressionModel.OnValidationCheck -= OnExpressionValidationCheck;
            AppResources.OnGraphColorCahnged -= OnGraphColorChanged;
            
            Closed -= OnWindowClosed;
        }

        
    }
}