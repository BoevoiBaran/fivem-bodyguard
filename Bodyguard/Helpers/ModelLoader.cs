using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;

// ReSharper disable once CheckNamespace
namespace Client.Helpers
{
    public static class ModelLoader
    {
        public static async Task<bool> LoadModel(uint modelHash)
        {
            if (!API.IsModelInCdimage(modelHash))
            {
                Debug.WriteLine($"Invalid model {modelHash} was supplied to LoadModel.");
                return false;
            }
            
            API.RequestModel(modelHash);
            
            while (!API.HasModelLoaded(modelHash))
            {
                Debug.WriteLine($"Waiting for model {modelHash} to load");
                await BaseScript.Delay(100);
            }
            
            return true;
        }
    }
}