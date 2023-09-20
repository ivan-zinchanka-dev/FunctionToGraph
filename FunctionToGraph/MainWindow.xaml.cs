using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FunctionToGraph.Models;
using Color = System.Drawing.Color;
using Expression = NCalc.Expression;

namespace FunctionToGraph
{
    public partial class MainWindow : Window
    {
        private IntegerRange _plotRange = new IntegerRange(-10, 10);
        private const char XChar = 'x';
        
        public MainWindow()
        {
            InitializeComponent();
            
            
            //Expression exp = new Expression("Sqrt(25)");
            //Console.WriteLine(Convert.ToDouble(exp.Evaluate()));
            
            _plot.Plot.Title("Graph");
            _plot.Plot.XLabel("x");
            _plot.Plot.YLabel("y");
        }

        private void OnFunctionTextChanged(object sender, TextChangedEventArgs args)
        {
            
            // Sqrt(25), Pow(4,2), Log(2,4)
            
            try
            {
                double[] xValues = _plotRange.Generate().Select(x => (double)x).ToArray();
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
                _plot.Plot.AddScatter(xValues, yValues, Color.Red);
                _plot.Refresh();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error");
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
    }
}