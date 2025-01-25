using Domain.Models;

namespace FunctionCalculator.Handlers;

public class ExpressionHandler
{
    public GraphModel Handle(string expression)
    {
        ExpressionModel expressionModel = new ExpressionModel(expression);
        
        bool result = expressionModel.TryValidate(out string errorMessage);

        if (result)
        {
            return new GraphModel(
                expressionModel.FullExpressionString,
                expressionModel.XValues,
                expressionModel.YValues);
        }
        else
        {
            throw new Exception("Incorrect expression!");
        }
    }
}