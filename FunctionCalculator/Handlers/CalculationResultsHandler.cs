using System.Globalization;
using Domain.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace FunctionCalculator.Handlers;

public class CalculationResultsHandler
{
    private const string DefaultFolderName = "Results";
    private const string ResultsFileName = "results.csv";

    public void HandleAsync(GraphModel graphModel, string outputDirectoryPath)
    {
        if (graphModel == null)
        {
            throw new ArgumentNullException(nameof(graphModel));
        }

        DirectoryInfo outputDirectory = new DirectoryInfo(outputDirectoryPath ?? GetDefaultDirectoryPath());
        
        if (!outputDirectory.Exists)
        {
            outputDirectory.Create();
        }

        string resultsFilePath = Path.Combine(outputDirectory.FullName, ResultsFileName);

        FileInfo resultsFile = new FileInfo(resultsFilePath);
        bool appendMode = resultsFile.Exists && resultsFile.Length > 0;
        
        using (StreamWriter streamWriter = new StreamWriter(resultsFilePath, appendMode))
        {
            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = !appendMode,
            };
            
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfig, leaveOpen: true))
            {
                csvWriter.WriteRecords(ToCalculationRecords(graphModel));
                csvWriter.Flush();
            }
            
            streamWriter.Flush();
        }
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

    private static string GetDefaultDirectoryPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), DefaultFolderName);
    }
    
}