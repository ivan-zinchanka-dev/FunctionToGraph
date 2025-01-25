using Domain.Models;
using FunctionCalculator.Handlers;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use params
    // TODO use async
    // TODO add report.metadata.json (or xaml) to report.csv 
    // TypeConverter(typeof(ColorConverter))

    
    
    public async void Run(string expression, string outputDirectoryPath)
    {
        try
        {
            GraphModel graphModel = new ExpressionHandler().Handle(expression);
            
            new CalculationResultsHandler().HandleAsync(graphModel, outputDirectoryPath);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
        

        /*CsvConfiguration config = CsvConfiguration.FromAttributes<CalculationRecord>();
        
        using (StreamReader streamReader = new StreamReader("table.csv"))
        {
            using (CsvReader csvReader = new CsvReader(streamReader, config))
            {
                IEnumerable<CalculationRecord> records = csvReader.GetRecords<CalculationRecord>();

                foreach (var record in records)
                {
                    Console.WriteLine(record);
                }
            }
        }*/

        //Console.WriteLine("CSV ready");
    }


    private async Task HandleErrorAsync(Exception ex)
    {
        ConsoleColor defaultColor = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Red;
        await Console.Error.WriteLineAsync(ex.Message);
        Console.ForegroundColor = defaultColor;
    }
}