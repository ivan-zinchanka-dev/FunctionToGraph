using Domain.Models;
using Domain.Storage;
using FunctionCalculator.Handlers;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use params

    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
    
    public async Task Run(string expression, string outputDirectoryPath)
    {
        try
        {
            GraphModel graphModel = new ExpressionHandler().Handle(expression);
            await new CalculationResultsHandler().HandleAsync(graphModel, outputDirectoryPath);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
    
    private async Task HandleErrorAsync(Exception ex)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Error.WriteLineAsync(ex.Message);
        Console.ForegroundColor = defaultColor;
    }
}