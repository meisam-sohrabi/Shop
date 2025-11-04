namespace LogService
{
    public class LogAppService : ILogAppService
    {
        public async Task LogAsync(string message, string appServiceName, string currentuserId)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Log");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var logMessage = $"time :[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] message: {message} user id: {currentuserId} {Environment.NewLine}";
            var fileName = appServiceName + "Log" + ".txt";
            if (File.Exists(Path.Combine(path, fileName)))
            {
                await File.AppendAllTextAsync(Path.Combine(path, fileName), logMessage);

            }
        }
    }
}
