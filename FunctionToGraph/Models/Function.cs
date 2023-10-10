using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NCalc;

namespace FunctionToGraph.Models;

public class Function : IDataErrorInfo
{
    private Range _plotRange = new Range(-10, 10, 60);
    
    private const char XChar = 'x';
    
    public string Error { get; }

    public string ExpressionString { get; set; }

    public double[] XValues { get; private set; }
    public double[] YValues { get; private set; }

    public event Action<Function, bool> OnValidationCheck; 

    public string this[string columnName]
    {
        get
        {
            Console.WriteLine("Column:" + columnName);
            
            if (columnName == nameof(ExpressionString))
            {

                //ExpressionString += "_";
                
                try
                {
                    XValues = _plotRange.Generate().ToArray();
                    YValues = new double[XValues.Length];

                    for (int i = 0; i < YValues.Length; i++)
                    {
                        double? result = GetY(XValues[i]);

                        if (result == null)
                        {
                            FireOnValidationCheck(false);
                            return "Incorrect expression.";
                        }

                        YValues[i] = result.Value;
                    
                    }

                }
                catch (Exception ex)
                {
                    FireOnValidationCheck(false);
                    return "Incorrect expression.";
                }
                
                
                /*Console.WriteLine("ExpressionString");

                return "Incorrect expression.";*/
            }
            
            FireOnValidationCheck(true);
            return string.Empty;
            
        }
    }

    private void FireOnValidationCheck(bool validationResult)
    {
        OnValidationCheck?.Invoke(this, validationResult);
    }

    private double? GetY(double x)
    {
            
        string expressionString = ExpressionString;

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