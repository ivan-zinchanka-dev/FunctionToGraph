using System.Drawing;
using Newtonsoft.Json;

namespace Domain.Storage;

internal class GraphColorStorage
{
    private readonly FileInfo _graphColorsFile;
    private Dictionary<string, Color> _graphColors = new Dictionary<string, Color>();
    public bool IsInitialized = false;
    
    public GraphColorStorage(string graphColorsFilePath)
    {
        _graphColorsFile = new FileInfo(graphColorsFilePath);
    }

    public async Task InitializeAsync()
    {
        _graphColors = await GetColorsDictionaryAsync();
        IsInitialized = true;
    }

    public Color GetGraphColor(string expression)
    {
        return _graphColors.TryGetValue(expression, out Color color) ? color : default;
    }
    
    public void SetGraphColor(string expression, Color color)
    {
        _graphColors[expression] = color;
    }

    public async Task SaveAsync()
    {
        await SaveColorsDictionaryAsync();
    }

    private async Task<Dictionary<string, Color>> GetColorsDictionaryAsync()
    {
        if (_graphColorsFile.Exists && _graphColorsFile.Length > 0)
        {
            string jsonNotation = await File.ReadAllTextAsync(_graphColorsFile.FullName);
            return JsonConvert.DeserializeObject<Dictionary<string, Color>>(jsonNotation);
        }
        else
        {
            return new Dictionary<string, Color>();
        }
    }

    private async Task SaveColorsDictionaryAsync()
    {
        string jsonNotation = JsonConvert.SerializeObject(_graphColors);
        await File.WriteAllTextAsync(_graphColorsFile.FullName, jsonNotation);
    }
}