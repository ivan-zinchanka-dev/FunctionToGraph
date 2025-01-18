using System.ComponentModel;
using System.Globalization;
using NCalc;

namespace Common.Models
{
    public class ExpressionModel : IDataErrorInfo
    {
        private const char XChar = 'x';
        public const string IncorrectExpressionMessage = "Incorrect expression";
        
        private Range _plotRange = new Range(-20, 20, 2000);

        public bool IsValidated { get; private set; }
        public string ExpressionString { get; set; }
        public string FullExpressionString => "y=" + ExpressionString;
        
        public double[] XValues { get; private set; }
        public double[] YValues { get; private set; }

        public event Action<ExpressionModel, bool> OnValidationCheck;
        public string Error => string.Empty;

        public ExpressionModel(string expressionString)
        {
            ExpressionString = expressionString;
        }

        public bool Validate(out string errorMessage)
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
                        errorMessage = IncorrectExpressionMessage;
                        return false;
                    }

                    YValues[i] = result.Value;
                }

                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                FireOnValidationCheck(false);
                errorMessage = IncorrectExpressionMessage;
                return false;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(ExpressionString))
                {
                    Validate(out string errorMessage);
                    return errorMessage;
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
                
                if (ExpressionString[i] == XChar && i < ExpressionString.Length - 1 
                                                 && IsAsciiDigitOrX(ExpressionString[Math.Min(i + 1, ExpressionString.Length - 1)]))
                {
                    ExpressionString = ExpressionString.Insert(i + 1, "*");
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
            IsValidated = validationResult;
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
    
}

