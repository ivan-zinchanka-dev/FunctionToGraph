using Domain.Models;
using Domain.Storage;
using FunctionCalculator.Handlers;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use params
    // TODO use async
    // TODO add report.metadata.json (or xaml) to report.csv 
    // TypeConverter(typeof(ColorConverter))

    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
    
    public async void Run(string expression, string outputDirectoryPath)
    {
        try
        {
            GraphModel graphModel = new ExpressionHandler().Handle(expression);
            new CalculationResultsHandler().Handle(graphModel, outputDirectoryPath);
            
            /*StorageService ss = new StorageService(GetDefaultDirectoryPath());
            var models = ss.GetGraphModels();

            foreach (var model in models)
            {
                Console.WriteLine(model.FullExpression + " " + model.XValues.Length + " " + model.YValues.Length);
            }*/

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