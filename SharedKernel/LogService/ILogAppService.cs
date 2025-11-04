namespace LogService
{
    public interface ILogAppService
    {
        Task LogAsync(string message, string appServiceName, string currentuserId);
    }
}
