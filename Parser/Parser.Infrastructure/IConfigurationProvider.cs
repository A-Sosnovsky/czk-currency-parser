namespace Parser.Infrastructure
{
    public interface IConfigurationProvider
    {
        string GetConfigurationValue(string key);
    }
}