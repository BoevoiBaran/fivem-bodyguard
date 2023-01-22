using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace Client
{
    public static class ConfigLoader
    {
        public static BodyguardConfig GetConfig()
        {
            var resourceName = API.GetCurrentResourceName();
            Debug.WriteLine($"[ConfigLoader] {resourceName}");
            
            var json = API.LoadResourceFile(resourceName, "config.json");
            Debug.WriteLine($"[ConfigLoader] {json}");
            
            var config = JsonConvert.DeserializeObject<BodyguardConfig>(json);
            Debug.WriteLine($"[ConfigLoader] Beh:{config.CombatBehaviour} Ammo:{config.AmmoCount}");

            return config;
        }
    }

    [JsonObject]
    public class BodyguardConfig
    {
        [JsonProperty("combat_behaviour")] public int CombatBehaviour { get; protected set; } = 2;
        [JsonProperty("ammo_count")] public int AmmoCount { get; protected set; } = 100;
        
        public BodyguardConfig() { }

        public static BodyguardConfig GetDefaultCfg()
        {
            return new BodyguardConfig();
        }
    }
}