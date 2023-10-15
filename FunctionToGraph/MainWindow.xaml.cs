using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private readonly ObservableCollection<GraphModel> _fixedGraphModels;

        public MainWindow()
        {
            InitializeComponent();
            
            Closed += OnWindowClosed;
            Loaded += OnWindowLoaded;
            
            _expressionModel = (ExpressionModel)Resources["ExpressionModel"];

            /*_fixedGraphModels.Add(new GraphModel()
            {
                ExpressionString = "2*x",
                Color = System.Drawing.Color.Aqua,
            });
            
            _fixedGraphModels.Add(new GraphModel()
            {
                ExpressionString = "Sin(x)",
                Color = System.Drawing.Color.Chocolate,
            });*/

            _fixedGraphModels = new ObservableCollection<GraphModel>(StorageUtility.ReadGraphModels());
            _fixedGraphModels.CollectionChanged += UpdateGraphModelsStorage;
            _graphsListView.ItemsSource = _fixedGraphModels;
        }

        private void OnWindowLoaded(object sender, EventArgs args)
        {
            _plot.Plot.Title("Graph");
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");
            
            _expressionModel.OnValidationCheck += OnExpressionValidationCheck;
            AppResources.OnGraphColorCahnged += OnGraphColorChanged;
            
            RedrawScatterPlot();
        }

        private void UpdateGraphModelsStorage(object? sender, NotifyCollectionChangedEventArgs args)
        {
            StorageUtility.SaveGraphModels(_fixedGraphModels);
        }

        /*private void RedrawScatterPlot()
        {
            _plot.Plot.Clear();
            _plot.Plot.AddScatter(_expressionModel.XValues, _expressionModel.YValues, 
                AppResources.GraphColor.ToDotNetColor(), 2.0f, 0.0f, MarkerShape.none,
                LineStyle.Solid, _expressionModel.FullExpressionString);
            
            _plot.Plot.Legend();
            _plot.Refresh();
        }*/
        
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
                _fixedGraphModels.Add(new GraphModel() {
                    
                    ExpressionString = _expressionModel.ExpressionString, 
                    Color = AppResources.GraphColor.ToDotNetColor(),
                    XValues = _expressionModel.XValues,
                    YValues = _expressionModel.YValues,
                    
                });
            }
        }
        
        private void OnGraphColorButtonClick(object sender, RoutedEventArgs e)
        {
            GraphColorWindow graphColorWindow = new GraphColorWindow();
            graphColorWindow.Show();
        }
        
        private void OnWindowClosed(object sender, EventArgs args)
        {
            _expressionModel.OnValidationCheck -= OnExpressionValidationCheck;
            AppResources.OnGraphColorCahnged -= OnGraphColorChanged;
            
            Loaded -= OnWindowLoaded;
            Closed -= OnWindowClosed;
        }

        
    }
}