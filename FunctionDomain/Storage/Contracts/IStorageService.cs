using FunctionDomain.Models;

namespace FunctionDomain.Storage.Contracts;

public interface IStorageService
{
    Task<IEnumerable<GraphModel>> GetGraphModelsAsync();
    Task SaveGraphModelsAsync(IEnumerable<GraphModel> graphModels);
    Task SaveGraphModelAsync(GraphModel graphModel);
}