using System.Collections.Generic;
using System.IO;
using CitizenFX.Core;
using Client.Helpers;

// ReSharper disable once CheckNamespace
namespace Client
{
    public static class ConfigLoader
    {
        public static BodyguardConfig GetConfig()
        {
            Debug.WriteLine($"[ConfigLoader] Try getCurrent directory");
            
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = $"{currentDirectory}\\Cfg";
            
            Debug.WriteLine($"[ConfigLoader] cfg path:{path}");
            
            var cfgData = DataProvider.GetData(path);
            var config = new BodyguardConfig(cfgData);
            return config;
        }
    }

    public class BodyguardConfig
    {
        private BodyguardConfig()
        {
            
        }
        
        public BodyguardConfig(Dictionary<string, Dictionary<string, object>> cfgData)
        {
            
        }

        public static BodyguardConfig GetDefaultCfg()
        {
            return new BodyguardConfig();
        }
    }
}