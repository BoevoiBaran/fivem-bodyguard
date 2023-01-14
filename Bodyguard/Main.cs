using System;
using System.Threading.Tasks;
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
            _manager = new BodyguardsManager();
            
            Debug.WriteLine("[Bodyguard] Dll command register");
            
            Update();
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
                await SpawnBodyguard();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"[Bodyguard] Exception:{e}");    
            }
        }

        private async Task SpawnBodyguard()
        {
            Debug.WriteLine("[Bodyguard] Try spawn bodyguard");
            
            var owner = Game.Player.Character;
            var bodyGuardHash = PedHash.ChemSec01SMM;
            var bodyGuardHashUint = (uint) bodyGuardHash; 
            
            API.RequestModel(bodyGuardHashUint);
            while (!API.HasModelLoaded(bodyGuardHashUint))
            {
                await Delay(100);
            }
            
            var bodyguardPed = await World.CreatePed(bodyGuardHash, owner.Position + owner.ForwardVector * 2);
            var bodyGuard = new Bodyguard(bodyguardPed, owner);
            _manager.AddBodyguard(bodyGuard);

            Debug.WriteLine("[Bodyguard] Bodyguard spawn finished");
        }
    }
} 