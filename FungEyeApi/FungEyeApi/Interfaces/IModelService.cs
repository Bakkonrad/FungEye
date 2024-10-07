namespace FungEyeApi.Interfaces
{
    public interface IModelService
    {
        Task<List<(string, double)>> Predict(IFormFile file);
    }
}
