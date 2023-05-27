namespace VrisingApi
{
    public class Configuration : IConfig
    {
        public string VrisingLogPath => Environment.GetEnvironmentVariable("LOG_PATH") ?? string.Empty;
    }
}
