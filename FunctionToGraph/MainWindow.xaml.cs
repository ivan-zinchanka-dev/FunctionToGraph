using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FunctionToGraph.Extensions;
using FunctionToGraph.Models;
using FunctionToGraph.Resources.Logical;
using FunctionToGraph.Views;
using Color = System.Windows.Media.Color;
using Expression = NCalc.Expression;
using Range = FunctionToGraph.Models.Range;

namespace FunctionToGraph
{
    public partial class MainWindow : Window
    {
        private Range _plotRange = new Range(-10, 10, 60);
        private const char XChar = 'x';

        private Function _function;
        
        public MainWindow()
        {
            InitializeComponent();
            
            Closed += OnWindowClosed;
            
            //Expression exp = new Expression("Sqrt(25)");
            //Console.WriteLine(Convert.ToDouble(exp.Evaluate()));
            
            _plot.Plot.Title("Graph");
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");

            _function = (Function)Resources["Function"];

            _function.OnValidationCheck += OnExpressionValidationCheck;
            AppResources.OnGraphColorCahnged += OnGraphColorChanged;
            
        }

        private void OnExpressionValidationCheck(Function function, bool validationResult)
        {
            if (validationResult)
            {
                _plot.Plot.Clear();
                _plot.Plot.AddSignal(function.YValues, 1, AppResources.GraphColor.ToDotNetColor());
                _plot.Refresh();
            }
            else
            {
                _plot.Plot.Clear();
                _plot.Refresh();
            }
            
        }

        private void OnGraphColorChanged(Color color)
        {
            _plot.Plot.Clear();
            _plot.Plot.AddSignal(_function.YValues, 1, AppResources.GraphColor.ToDotNetColor());
            _plot.Refresh();
        }
        
        private void OnGraphColorButtonClick(object sender, RoutedEventArgs e)
        {
            GraphColorWindow graphColorWindow = new GraphColorWindow();
            graphColorWindow.Show();
        }
        
        private void OnWindowClosed(object sender, EventArgs args)
        {
            _function.OnValidationCheck -= OnExpressionValidationCheck;
            AppResources.OnGraphColorCahnged -= OnGraphColorChanged;
            Closed -= OnWindowClosed;
        }
    }
}