namespace JERP.Desktop.Services;

public interface IApiClient
{
    void SetAuthToken(string? token);
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<T?> PutAsync<T>(string endpoint, object data);
    Task DeleteAsync(string endpoint);
    Task<byte[]> GetBytesAsync(string endpoint);
}
