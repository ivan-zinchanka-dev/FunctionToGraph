using System;
using System.Collections.Generic;
using System.IO;
using FunctionToGraph.Models;
using Newtonsoft.Json;

namespace FunctionToGraph.Utilities;

public static class StorageUtility
{
    private const string AppFolderName = "FunctionToGraph";
    
    private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

    private const string GraphModelsFileName = "graph_models.json";
    
    //C:\Users\HP\AppData\Roaming

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
    
    
    public static void SaveGraphModels(IEnumerable<GraphModel> graphModels)
    {
        string jsonNotation = JsonConvert.SerializeObject(graphModels, Formatting.Indented);
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);
        
        File.WriteAllText(fullFileName, jsonNotation);
    }
    
    public static IEnumerable<GraphModel> ReadGraphModels()
    {
        string fullFileName = Path.Combine(_appDirectoryInfo.FullName, GraphModelsFileName);
        string jsonNotation = File.ReadAllText(fullFileName);

        IEnumerable<GraphModel> graphModels = JsonConvert.DeserializeObject<IEnumerable<GraphModel>>(jsonNotation);

        return graphModels ?? new List<GraphModel>();
    }
    
    public static void Test()
    {
        string path = Path.Combine(AppDataPath, AppFolderName);
        
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }

        FileInfo fileInfo = new FileInfo(Path.Combine(path, "some_file.txt"));

        if (!fileInfo.Exists)
        {
            fileInfo.Create();
        }

    }
    
}