using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using NCalc;

namespace FunctionToGraph.Models;

public class ExpressionModel : IDataErrorInfo
{
    private const char XChar = 'x';
    private const string IncorrectExpressionMessage = "Incorrect expression";
    
    private Range _plotRange = new Range(-10, 10, 160);
    
    public string ExpressionString { get; set; }
    public double[] XValues { get; private set; }
    public double[] YValues { get; private set; }

    public event Action<ExpressionModel, bool> OnValidationCheck;
    public string Error => string.Empty;
    
    public string this[string columnName]
    {
        get
        {
            if (columnName == nameof(ExpressionString))
            {
                try
                {
                    CorrectExpressionIfNeed();
                    
                    XValues = _plotRange.Generate().ToArray();
                    YValues = new double[XValues.Length];

                    for (int i = 0; i < YValues.Length; i++)
                    {
                        double? result = GetY(XValues[i]);

                        if (result == null)
                        {
                            FireOnValidationCheck(false);
                            return IncorrectExpressionMessage;
                        }

                        YValues[i] = result.Value;
                    }

                }
                catch (Exception ex)
                {
                    FireOnValidationCheck(false);
                    return IncorrectExpressionMessage;
                }
            }
            
            FireOnValidationCheck(true);
            return string.Empty;
            
        }
    }

    private void CorrectExpressionIfNeed()
    {
        for (int i = 1; i < ExpressionString.Length; i++)
        {
            if (ExpressionString[i] == XChar && IsAsciiDigitOrX(ExpressionString[i - 1]))
            {
                ExpressionString = ExpressionString.Insert(i, "*");
            }
            
            if (ExpressionString[i] == XChar && ExpressionString[i - 1] == '-')
            {
                ExpressionString = ExpressionString.Insert(i, "1*");
            }
        }
    }
    
    private static bool IsAsciiDigitOrX(char symbol)
    {
        return symbol == XChar || char.IsAscii(symbol) && char.IsDigit(symbol);
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
        
        for (int i = 0; i < expressionString.Length; i++)
        {
            if (expressionString[i] == XChar)
            {
                expressionString = expressionString
                    .Remove(i, 1)
                    .Insert(i, x.ToString(CultureInfo.InvariantCulture));
            }
        }
        
        Expression expression = new Expression(expressionString);

        if (expression.HasErrors())
        {
            return null;
        }

        return Convert.ToDouble(expression.Evaluate());
    }
}