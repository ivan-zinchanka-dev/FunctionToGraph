using System.Data;
using Domain.Csv;
using Domain.Extensions;
using Domain.Factories;
using Domain.Models;

namespace Domain.Storage;

public class StorageService
{
    private const string GraphsFolder = "Graphs";
    private const string GraphModelsFileName = "models.csv";
    private const string GraphColorsFileName = "colors.json";
    
    private readonly DirectoryInfo _storageDirectory;
    private readonly string _graphModelsFilePath;
    private readonly GraphColorStorage _graphColorStorage;
    
    public StorageService(string storageDirectoryPath)
    {
        if (storageDirectoryPath == null)
        {
            throw new ArgumentNullException(nameof(storageDirectoryPath));
        }
        
        _storageDirectory = new DirectoryInfo(Path.Combine(storageDirectoryPath, GraphsFolder));
        CheckStorageDirectory();
        
        _graphModelsFilePath = Path.Combine(_storageDirectory.FullName, GraphModelsFileName);
        string graphColorsFilePath = Path.Combine(_storageDirectory.FullName, GraphColorsFileName);
        
        _graphColorStorage = new GraphColorStorage(graphColorsFilePath);
    }
    
    public async Task<IEnumerable<GraphModel>> GetGraphModelsAsync()
    {
        FileInfo graphsFile = new FileInfo(_graphModelsFilePath);

        if (graphsFile.Exists && graphsFile.Length > 0)
        {
            CsvReader csvReader = new CsvReader();
            DataTable dataTable = await csvReader.ReadDataAsync(graphsFile.FullName);
        
            IEnumerable<GraphModel> models = GraphModelParser.Parse(dataTable);
            await CheckGraphColorStorageAsync();
            
            foreach (GraphModel model in models)
            {
                model.Color = _graphColorStorage.GetGraphColor(model.Expression);
            }
            
            return models;
        }

        return new List<GraphModel>();
    }
    
    public async Task SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels)
    {
        CheckStorageDirectory();
        await CheckGraphColorStorageAsync();
        
        CsvWriter csvWriter = new CsvWriter();
        
        foreach (GraphModel graphModel in graphModels)
        {
            await csvWriter.WriteDataAsync(_graphModelsFilePath, graphModel.ToDataTable(), true);
            _graphColorStorage.SetGraphColor(graphModel.Expression, graphModel.Color);
        }

        await _graphColorStorage.SaveAsync();
    }
    
    public async Task SaveGraphModelAsync(GraphModel graphModel)
    {
        await SaveGraphModelsAsync(new []{ graphModel });
    }
    
    private void CheckStorageDirectory()
    {
        if (!_storageDirectory.Exists)
        {
            _storageDirectory.Create();
        }
    }
    
    private async Task CheckGraphColorStorageAsync()
    {
        if (!_graphColorStorage.IsInitialized)
        {
            await _graphColorStorage.InitializeAsync();
        }
    }
}