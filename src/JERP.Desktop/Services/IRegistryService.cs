namespace JERP.Desktop.Services;

public interface IRegistryService
{
    string? GetValue(string key, string? defaultValue = null);
    void SetValue(string key, string value);
    void DeleteValue(string key);
    string? GetApiUrl();
    void SetApiUrl(string url);
    string? GetRememberedUsername();
    void SetRememberedUsername(string username);
    void ClearRememberedUsername();
}
