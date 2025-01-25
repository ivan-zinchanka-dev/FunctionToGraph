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

    public void SaveGraphModelsAsync(GraphModel graphModel)
    {
        SaveGraphModelsAsync(new []{ graphModel });
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
    
    private IEnumerable<GraphModel> ToGraphModels(IEnumerable<CalculationRecord> records)
    {
        IEnumerable<IGrouping<string, CalculationRecord>> recordGroups = records.GroupBy(record => record.Expression);
        
        List<GraphModel> graphModels = new List<GraphModel>();
        
        foreach (IGrouping<string, CalculationRecord> recordGroup in recordGroups)
        {
            graphModels.Add(ToGraphModel(recordGroup.Key, recordGroup));
        }
        
        return graphModels;
    }

    private GraphModel ToGraphModel(string recordsKey, IEnumerable<CalculationRecord> records)
    {
        List<CalculationRecord> recordsList = records.ToList();
        
        double[] xValues = new double[recordsList.Count];
        double[] yValues = new double[recordsList.Count];

        for (int i = 0; i < xValues.Length; i++)
        {
            xValues[i] = recordsList[i].XValue;
        }
        
        for (int i = 0; i < yValues.Length; i++)
        {
            yValues[i] = recordsList[i].YValue;
        }

        return new GraphModel(recordsKey, xValues, yValues);
    }

    //TODO Async
    public IEnumerable<GraphModel> GetGraphModelsAsync()
    {
        CheckStorageDirectory();
        
        string graphsFilePath = Path.Combine(_storageDirectory.FullName, GraphModelsFileName);
        
        CsvConfiguration csvConfig = CsvConfiguration.FromAttributes<CalculationRecord>();

        IEnumerable<GraphModel> graphModel;
        
        using (StreamReader streamReader = new StreamReader(graphsFilePath))
        {
            using (CsvReader csvReader = new CsvReader(streamReader, csvConfig))
            {
                IEnumerable<CalculationRecord> records = csvReader.GetRecords<CalculationRecord>();
                graphModel = ToGraphModels(records);
            }
        }
        
        return graphModel;
    }
    
    private void CheckStorageDirectory()
    {
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
    }
}