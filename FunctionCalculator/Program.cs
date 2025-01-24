using System.CommandLine;
using System.ComponentModel.DataAnnotations;

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


        CalculationRunner calculationRunner = new CalculationRunner();
        rootCommand.SetHandler(calculationRunner.Run, expressionOption, outputDirectoryOption);
        
        return await rootCommand.InvokeAsync(args);
    }

}