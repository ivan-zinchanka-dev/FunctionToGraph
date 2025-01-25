using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;

namespace Domain.Storage;

public class StorageService
{
    //private const string AppFolderName = "FunctionToGraph";

    private const string GraphsFolder = "Graphs";
    private const string GraphModelsFileName = "graphs.csv";
    //private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    
    private readonly DirectoryInfo _storageDirectory;

    public StorageService(string storageDirectoryPath)
    {
        if (storageDirectoryPath == null)
        {
            throw new ArgumentNullException(nameof(storageDirectoryPath));
        }

        _storageDirectory = new DirectoryInfo(Path.Combine(storageDirectoryPath, GraphsFolder));
        CheckStorageDirectory();
    }
    
    // TODO Async
    public void SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels)
    {
        CheckStorageDirectory();
        
        string graphsFilePath = Path.Combine(_storageDirectory.FullName, GraphModelsFileName);
        
        bool appendMode = GetAppendMode(graphsFilePath);
        
        CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = !appendMode,
        };
        
        using (StreamWriter streamWriter = new StreamWriter(graphsFilePath, appendMode))
        {
            using (CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfig, leaveOpen: true))
            {
                csvWriter.WriteRecords(ToCalculationRecords(graphModels));
                csvWriter.Flush();
            }
            
            streamWriter.Flush();
        }
    }
    
    private static bool GetAppendMode(string filePath)
    {
        FileInfo file = new FileInfo(filePath);
        return file.Exists && file.Length > 0;
    }
    
    private IEnumerable<CalculationRecord> ToCalculationRecords(IEnumerable<GraphModel> graphModels)
    {
        return graphModels.SelectMany(ToCalculationRecords);
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

    public async Task<IEnumerable<GraphModel>> ReadGraphModelsAsync()
    {
        return null;
    }
    
    private void CheckStorageDirectory()
    {
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
    }
}