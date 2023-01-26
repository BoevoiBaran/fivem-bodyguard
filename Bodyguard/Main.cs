using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class Main : BaseScript
    {
        private readonly BodyguardsManager _manager;
        
        public Main()
        {
            Debug.WriteLine("[Bodyguard] Dll loaded");
            
            API.RegisterCommand("spawn_bodyguard", new Action(SpawnBodyguardCommand), false);
            
            _manager = new BodyguardsManager(GetConfig());
            
            Debug.WriteLine("[Bodyguard] Dll command register");
            
            Update();
        }

        private static BodyguardConfig GetConfig()
        {
            var config = BodyguardConfig.GetDefaultCfg();
            
            try
            {
                config = ConfigLoader.GetConfig();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Bodyguard] ConfigLoader exception:{e}. Use default config");
            }

            return config;
        }
        
        private async void Update()
        {
            Debug.WriteLine("[Bodyguard] Start update loop");
            
            while (true)
            {
                _manager?.Update();        
                await Delay(200);
            }
        }

        private async void SpawnBodyguardCommand()
        {
            try
            {
                await _manager.SpawnBodyguard();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Bodyguard] Exception:{e}");    
            }
        }
    }
} 