using Domain.Models;
using Domain.Storage;

namespace FunctionCalculator.Handlers;

public class CalculationResultsHandler
{
    public void Handle(GraphModel graphModel, string outputDirectoryPath)
    {
        if (graphModel == null)
        {
            throw new ArgumentNullException(nameof(graphModel));
        }
        
        StorageService storageService = new StorageService(outputDirectoryPath ?? GetDefaultDirectoryPath());
        
        storageService.SaveGraphModel(graphModel);
    }
    
    private static string GetDefaultDirectoryPath()
    {
        return Directory.GetCurrentDirectory();
    }
}