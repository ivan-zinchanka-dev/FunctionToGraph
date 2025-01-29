using System.CommandLine;

namespace FunctionCalculator;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        RootCommand rootCommand = new RootCommand("This calculator calculates function values");

        Option<string> expressionOption =
            new Option<string>("--exp", "Specifies the expression, that represents function");
        expressionOption.IsRequired = true;
        rootCommand.AddOption(expressionOption);
        
        Option<string> outputDirectoryOption =
            new Option<string>("--out", "Specifies the output directory");
        rootCommand.AddOption(outputDirectoryOption);
        
        CalculationRunner runner = new CalculationRunner();
        rootCommand.SetHandler((Func<string, string, Task>) (async (expression, outputDirectory) =>
        {
            await runner.Run(expression, outputDirectory);
        }), expressionOption, outputDirectoryOption);
        
        return await rootCommand.InvokeAsync(args);
    }
}