using Domain.Models;
using Domain.Storage;

namespace FunctionCalculator.Handlers;

public class CalculationResultsHandler
{
    public async void Handle(GraphModel graphModel, string outputDirectoryPath)
    {
        if (graphModel == null)
        {
            throw new ArgumentNullException(nameof(graphModel));
        }
        
        StorageService storageService = new StorageService(outputDirectoryPath ?? GetDefaultDirectoryPath());
        
        await storageService.SaveGraphModelAsync(graphModel);
    }
    
    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
}