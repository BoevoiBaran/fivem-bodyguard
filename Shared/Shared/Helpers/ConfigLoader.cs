using System.Collections.Generic;
using CitizenFX.Core;

namespace Shared.Shared.Helpers
{
    public static class DataProvider
    {
        public static Dictionary<string, Dictionary<string, object>> GetData(string dataPath)
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            if (!System.IO.Directory.Exists(dataPath))
            {
                return result;
            }

            ProcessDirectory(dataPath, result);

            return result;
        }

        private static void ProcessDirectory(string path, Dictionary<string, Dictionary<string, object>> container)
        {
            string [] fileEntries = System.IO.Directory.GetFiles(path);
            string [] directoryEntries = System.IO.Directory.GetDirectories(path);
            foreach (var fileEntry in fileEntries)
            {
                if (string.Equals(System.IO.Path.GetExtension(fileEntry), ".json"))
                {
                    ReadFile(fileEntry, container);
                }
            }

            foreach (var directoryPath in directoryEntries)
            {
                ProcessDirectory(directoryPath, container);
            }
        }
    
        private static void ReadFile(string filePath, Dictionary<string, Dictionary<string, object>> container)
        {
            Dictionary<string, object> dataNode = null;
            using (var sr = new System.IO.StreamReader(filePath))
            {
                var text = sr.ReadToEnd();
                dataNode = (Dictionary<string, object>) fastJSON.JSON.Parse(text);
            }
            var fileName = System.IO.Path.GetFileName(filePath);
            container[fileName] = dataNode;
        }
    }
}