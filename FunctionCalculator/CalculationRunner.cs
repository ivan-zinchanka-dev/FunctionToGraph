using System.Drawing;
using System.Globalization;
using Common.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use params
    // TODO use async
    
    public void Run(string expression, string outputDirectory)
    {
        ExpressionModel expressionModel = new ExpressionModel("x*(x-2)");
        
        bool result = expressionModel.TryValidate(out string errorMessage);

        Console.WriteLine("Result: " + result);

        GraphModel graphModel = new GraphModel(
            expressionModel.FullExpressionString,
            expressionModel.XValues,
            expressionModel.YValues,
            Color.Red);

        using (StreamWriter streamWriter = new StreamWriter("table.csv", false))
        {
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                //csvWriter.WriteComment("This is a metadata ta ta dada tata\n");
                csvWriter.WriteRecords(ToCalculationRecords(graphModel));
                csvWriter.Flush();
            }
        }


        /*CsvConfiguration config = CsvConfiguration.FromAttributes<CalculationRecord>();
        
        using (StreamReader streamReader = new StreamReader("in.csv"))
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

        Console.WriteLine("CSV ready");
    }

    private IEnumerable<CalculationRecord> ToCalculationRecords(GraphModel graphModel)
    {
        CalculationRecord[] records = new CalculationRecord[
            Math.Min(graphModel.XValues.Length, graphModel.YValues.Length)];
        
        if (records.Length > 0)
        {
            records[0] = new CalculationRecord(
                graphModel.Expression, 
                graphModel.XValues[0], 
                graphModel.YValues[0],
                graphModel.Color);
        }
        
        for (int i = 1; i < records.Length; i++)
        {
            records[i] = new CalculationRecord(graphModel.XValues[i],  graphModel.YValues[i]);
        }

        return records;
    }
}