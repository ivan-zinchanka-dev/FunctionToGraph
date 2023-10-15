using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FunctionToGraph.Models;
using Newtonsoft.Json;

namespace FunctionToGraph.Utilities;

public static class StorageUtility
{
    private const string AppFolderName = "FunctionToGraph";
    private const string GraphModelsFileName = "graph_models.json";
    
    private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    
    private static DirectoryInfo _appDirectoryInfo;

    static StorageUtility()
    {
        CheckApplicationFolder();
    }
    
    private static void CheckApplicationFolder()
    {
        string path = Path.Combine(AppDataPath, AppFolderName);
        _appDirectoryInfo = new DirectoryInfo(path);
        
        if (!_appDirectoryInfo.Exists)
        {
            _appDirectoryInfo.Create();
        }
    }

    public static async void SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels)
    {
        string jsonNotation = JsonConvert.SerializeObject(graphModels, Formatting.Indented);
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);
        
        await File.WriteAllTextAsync(fullFileName, jsonNotation);
    }
    
    public static async Task<IEnumerable<GraphModel>> ReadGraphModelsAsync()
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
    
    /*public static IEnumerable<GraphModel> ReadGraphModels()
    {
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);

        if (!File.Exists(fullFileName))
        {
            return new List<GraphModel>();
        }

        string jsonNotation = File.ReadAllText(fullFileName);
        IEnumerable<GraphModel> graphModels = JsonConvert.DeserializeObject<IEnumerable<GraphModel>>(jsonNotation);

        return graphModels ?? new List<GraphModel>();
    }*/
    
}