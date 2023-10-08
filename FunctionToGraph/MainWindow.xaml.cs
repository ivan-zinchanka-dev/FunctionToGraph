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

        private FunctionGrapher _functionGrapher;
        
        public MainWindow()
        {
            InitializeComponent();
            
            Closed += OnWindowClosed;
            
            //Expression exp = new Expression("Sqrt(25)");
            //Console.WriteLine(Convert.ToDouble(exp.Evaluate()));
            
            _plot.Plot.Title("Graph");
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");

            //_functionGrapher = (FunctionGrapher)Resources["function_grapher"];

            AppResources.OnGraphColorCahnged += OnGraphColorChanged;

            
        }

        private void OnGraphColorChanged(Color color)
        {
            Console.WriteLine("Scatter");
            
            //_plot.Plot.Scatter();
            
            OnFunctionTextChanged(null, null);
            
        }


        private void OnFunctionTextChanged(object sender, TextChangedEventArgs args)
        {
            
            // Sqrt(25), Pow(4,2), Log(2,4)
            
            try
            {
                double[] xValues = _plotRange.Generate().ToArray();
                double[] yValues = new double[xValues.Length];

                for (int i = 0; i < yValues.Length; i++)
                {
                    double? result = GetY(xValues[i]);

                    if (result == null)
                    {
                        _plot.Plot.Clear();
                        _plot.Refresh();
                        return;
                    }

                    yValues[i] = result.Value;
                    
                }
                
                _plot.Plot.Clear();
                _plot.Plot.AddScatter(xValues, yValues, AppResources.GraphColor.ToDotNetColor());
                _plot.Refresh();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private double? GetY(double x)
        {
            
            string expressionString = _functionTextBox.Text;

            if (string.IsNullOrEmpty(expressionString))
            {
                return null;
            }

            int xIndex = expressionString.IndexOf(XChar);
            
            if (xIndex != -1)
            {
                expressionString = expressionString.Insert(xIndex + 1, x.ToString(CultureInfo.InvariantCulture));
                expressionString = expressionString.Remove(xIndex, 1);
            }
            
            //Console.WriteLine("exp: " + expressionString);
            
            Expression expression = new Expression(expressionString);

            if (expression.HasErrors())
            {
                return null;
            }

            return Convert.ToDouble(expression.Evaluate());
            
        }

        private void OnGraphColorButtonClick(object sender, RoutedEventArgs e)
        {
            GraphColorWindow graphColorWindow = new GraphColorWindow();
            graphColorWindow.Show();
        }
        
        private void OnWindowClosed(object sender, EventArgs args)
        {
            AppResources.OnGraphColorCahnged -= OnGraphColorChanged;
            Closed -= OnWindowClosed;
        }
    }
}