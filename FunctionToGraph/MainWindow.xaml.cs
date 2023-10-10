using System;
using System.Windows;
using FunctionToGraph.Extensions;
using FunctionToGraph.Models;
using FunctionToGraph.Resources.Logical;
using FunctionToGraph.Views;
using Color = System.Windows.Media.Color;

namespace FunctionToGraph
{
    public partial class MainWindow : Window
    {
        private ExpressionModel _expressionModel;
        
        public MainWindow()
        {
            InitializeComponent();
            
            Closed += OnWindowClosed;
            Loaded += OnWindowLoaded;
            
            _expressionModel = (ExpressionModel)Resources["ExpressionModel"];
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

        private void RedrawScatterPlot()
        {
            _plot.Plot.Clear();
            _plot.Plot.AddScatter(_expressionModel.XValues, _expressionModel.YValues, 
                AppResources.GraphColor.ToDotNetColor(), 2.0f, 0.0f);
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
            else
            {
                ClearScatterPlot();
            }
        }

        private void OnGraphColorChanged(Color color)
        {
            RedrawScatterPlot();
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