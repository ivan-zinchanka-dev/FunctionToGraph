using Domain.Models;
using Domain.Storage.Concrete;

namespace FunctionCalculator;

public class CalculationRunner
{
    public async Task Run(string expression, string outputDirectoryPath)
    {
        try
        {
            GraphModel graphModel = CreateGraphModel(expression);
            
            StorageService storageService = new StorageService(outputDirectoryPath ?? GetDefaultDirectoryPath(), false);
            await storageService.SaveGraphModelAsync(graphModel);
            
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    private GraphModel CreateGraphModel(string expression)
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
            throw new Exception($"Incorrect expression: {expression}.");
        }
    }
    
    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
    
    private async Task HandleErrorAsync(Exception ex)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Error.WriteLineAsync(ex.Message);
        Console.ForegroundColor = defaultColor;
    }
}