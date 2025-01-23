﻿using System.Drawing;
using System.Globalization;
using Common.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace FunctionCalculator;

public class CalculationRunner
{
    // TODO use params
    // TODO use async
    // TODO add report.metadata.json (or xaml) to report.csv 
    // TypeConverter(typeof(ColorConverter))
    
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

        /*using (StreamWriter streamWriter = new StreamWriter("table.csv", false))
        {
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                //csvWriter.WriteComment("This is a metadata ta ta dada tata\n");
                csvWriter.WriteRecords(ToCalculationRecords(graphModel));
                csvWriter.Flush();
            }
        }*/


        CsvConfiguration config = CsvConfiguration.FromAttributes<CalculationRecord>();
        
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
        }

        Console.WriteLine("CSV ready");
    }

    private IEnumerable<CalculationRecord> ToCalculationRecords(GraphModel graphModel)
    {
        CalculationRecord[] records = new CalculationRecord[
            Math.Min(graphModel.XValues.Length, graphModel.YValues.Length)];
        
        for (int i = 0; i < records.Length; i++)
        {
            records[i] = new CalculationRecord(graphModel.Expression, graphModel.XValues[i], graphModel.YValues[i]);
        }

        return records;
    }
}