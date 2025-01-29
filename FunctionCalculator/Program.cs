using System.CommandLine;

namespace FunctionCalculator;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("A calculator that evaluates function expressions");

        Option<string> expressionOption =
            new Option<string>("--exp", "The mathematical expression to evaluate");
        expressionOption.IsRequired = true;
        rootCommand.AddOption(expressionOption);
        
        Option<string> outputDirectoryOption =
            new Option<string>("--out", "Directory where output files will be saved");
        rootCommand.AddOption(outputDirectoryOption);
        
        CalculationRunner runner = new CalculationRunner();
        rootCommand.SetHandler((Func<string, string, Task>) (async (expression, outputDirectory) =>
        {
            await runner.Run(expression, outputDirectory);
        }), expressionOption, outputDirectoryOption);
        
        return await rootCommand.InvokeAsync(args);
    }
}