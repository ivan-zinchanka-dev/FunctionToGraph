using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FunctionToGraph.Models;
using Newtonsoft.Json;

namespace FunctionToGraph.Services;

public class StorageService
{
    private const string AppFolderName = "FunctionToGraph";
    private const string GraphModelsFileName = "graph_models.json";
    
    private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    
    private DirectoryInfo _appDirectoryInfo;

    public StorageService()
    {
        CheckApplicationFolder();
    }
    
    public async void SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels)
    {
        string jsonNotation = JsonConvert.SerializeObject(graphModels, Formatting.Indented);
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);
        
        await File.WriteAllTextAsync(fullFileName, jsonNotation);
    }
    
    public async Task<IEnumerable<GraphModel>> ReadGraphModelsAsync()
    {
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);

        if (!File.Exists(fullFileName))
        {
            return new List<GraphModel>();
        }

        string jsonNotation = await File.ReadAllTextAsync(fullFileName);
        IEnumerable<GraphModel> graphModels = JsonConvert.DeserializeObject<IEnumerable<GraphModel>>(jsonNotation);

        return graphModels ?? new List<GraphModel>();
    }
    
    private void CheckApplicationFolder()
    {
        string path = Path.Combine(AppDataPath, AppFolderName);
        _appDirectoryInfo = new DirectoryInfo(path);
        
        if (!_appDirectoryInfo.Exists)
        {
            _appDirectoryInfo.Create();
        }
    }
}