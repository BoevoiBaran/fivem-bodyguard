using System.Collections.Generic;
using System.IO;
using Shared.Shared.Helpers;

// ReSharper disable once CheckNamespace
namespace Client
{
    public static class ConfigLoader
    {
        public static BodyguardConfig GetConfig()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = $"{currentDirectory}\\Cfg";
            var cfgData = DataProvider.GetData(path);
            var config = new BodyguardConfig(cfgData);
            return config;
        }
    }

    public class BodyguardConfig
    {
        public BodyguardConfig(Dictionary<string, Dictionary<string, object>> cfgData)
        {
            
        }
    }
}