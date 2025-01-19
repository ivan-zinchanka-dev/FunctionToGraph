using System.Globalization;
using Common.Models;
using CsvHelper;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use async
    
    public void Run(string expression, string outputDirectory)
    {
        ExpressionModel expressionModel = new ExpressionModel("x*(x-2)");
        
        bool result = expressionModel.TryValidate(out string errorMessage);

        Console.WriteLine("Result: " + result);

        using (StreamWriter streamWriter = new StreamWriter("table.csv"))
        {
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(
                    ToCalculationRecords(expressionModel.XValues, expressionModel.YValues));
                csvWriter.Flush();
            }
        }

        Console.WriteLine("CSV ready");
    }

    private IEnumerable<CalculationRecord> ToCalculationRecords(double[] xValues, double[] yValues)
    {
        CalculationRecord[] records = new CalculationRecord[Math.Min(xValues.Length, yValues.Length)];

        for (int i = 0; i < records.Length; i++)
        {
            records[i] = new CalculationRecord(xValues[i], yValues[i]);
        }

        return records;
    }
}