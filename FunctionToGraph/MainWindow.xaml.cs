using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NCalc;
using Expression = NCalc.Expression;

namespace FunctionToGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable _dataTable = new DataTable();
        
        public MainWindow()
        {
            InitializeComponent();
            
            
            //Expression exp = new Expression("Sqrt(25)");
            //Console.WriteLine(Convert.ToDouble(exp.Evaluate()));
        }

        private void OnFunctionTextChanged(object sender, TextChangedEventArgs args)
        {
            
            // Sqrt(25), Pow(4,2), Log(2,4)
            
            //try
            //{
                double[] xValues = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                double[] yValues = new double[xValues.Length];

                for (int i = 0; i < yValues.Length; i++)
                {
                    double? result = GetY(xValues[i]);

                    if (result == null)
                    {
                        _plot.Plot.Clear();
                        return;
                    }

                    yValues[i] = result.Value;
                    
                }
                
                _plot.Plot.Clear();
                _plot.Plot.AddScatter(xValues, yValues);
                _plot.Refresh();

                //}
            /*catch (Exception ex)
            {
                Console.WriteLine("Error");
            }*/
        }

        private double? GetY(double x)
        {
            const char xChar = 'x';
            string expressionString = _functionTextBox.Text;

            if (string.IsNullOrEmpty(expressionString))
            {
                return null;
            }

            int xIndex = expressionString.IndexOf(xChar);
            
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