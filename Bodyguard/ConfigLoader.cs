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
            var json = API.LoadResourceFile(resourceName, "config.json");
            var config = JsonConvert.DeserializeObject<BodyguardConfig>(json);

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