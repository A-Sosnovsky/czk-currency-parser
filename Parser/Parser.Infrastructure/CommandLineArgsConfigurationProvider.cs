using System;
using System.Linq;

namespace Parser.Infrastructure
{
    internal class CommandLineArgsConfigurationProvider : IConfigurationProvider
    {
        public string GetConfigurationValue(string key)
        {
            return Environment.GetCommandLineArgs().FirstOrDefault(k => k == key);
        }
    }
}