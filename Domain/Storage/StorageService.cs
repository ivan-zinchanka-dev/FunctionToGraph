using System.Data;
using Domain.Csv;
using Domain.Extensions;
using Domain.Factories;
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
    
    public async Task SaveGraphModelAsync(GraphModel graphModel)
    {
        await SaveGraphModelsAsync(new []{ graphModel });
    }
    
    public async Task SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels)
    {
        CheckStorageDirectory();
        
        string graphsFilePath = Path.Combine(_storageDirectory.FullName, GraphModelsFileName);

        CsvWriter csvWriter = new CsvWriter();

        foreach (GraphModel graphModel in graphModels)
        {
            await csvWriter.WriteDataAsync(graphsFilePath, graphModel.ToDataTable(), true);
        }
    }
    
    public async Task<IEnumerable<GraphModel>> GetGraphModelsAsync()
    {
        string graphsFilePath = Path.Combine(_storageDirectory.FullName, GraphModelsFileName);

        FileInfo graphsFile = new FileInfo(graphsFilePath);

        if (graphsFile.Exists && graphsFile.Length > 0)
        {
            CsvReader csvReader = new CsvReader();
            DataTable dataTable = await csvReader.ReadDataAsync(graphsFilePath);
        
            return GraphModelParser.Parse(dataTable);
        }

        return new List<GraphModel>();
    }
    
    private void CheckStorageDirectory()
    {
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
    }
}