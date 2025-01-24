using System.Globalization;
using CsvHelper;

namespace FunctionCalculator.Handlers;

public class CalculationResultsHandler
{
    private const string DefaultFolderName = "Results";
    private const string ResultsFileName = "results.csv";

    public async Task HandleAsync(GraphModel graphModel, string outputDirectoryPath)
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
        
        using (StreamWriter streamWriter = new StreamWriter(resultsFilePath, true))
        {
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                await csvWriter.WriteRecordsAsync(ToCalculationRecords(graphModel));
                await csvWriter.FlushAsync();
            }
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