using Common.Models;

namespace FunctionCalculator;

public class CalculationRunner
{
    public void Run(string expression, string outputDirectory)
    {
        ExpressionModel expressionModel = new ExpressionModel("x*(x-2)");
        
        bool result = expressionModel.Validate(out string errorMessage);

        Console.WriteLine("Result: " + result);
        
    }
}