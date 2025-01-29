using Domain.Models;
using Domain.Storage;

namespace FunctionCalculator.Handlers;

public class CalculationResultsHandler
{
    public async Task HandleAsync(GraphModel graphModel, string outputDirectoryPath)
    {
        if (graphModel == null)
        {
            throw new ArgumentNullException(nameof(graphModel));
        }
        
        StorageService storageService = new StorageService(outputDirectoryPath ?? GetDefaultDirectoryPath(), false);
        
        await storageService.SaveGraphModelAsync(graphModel);
    }
    
    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
}