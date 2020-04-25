using System;

namespace Parser.Infrastructure
{
    internal class EnvironmentVariablesConfigurationProvider : IConfigurationProvider
    {
        public string GetConfigurationValue(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}