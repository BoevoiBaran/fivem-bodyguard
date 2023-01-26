using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Helpers;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class BodyguardsManager
    {
        private bool _started;
        private readonly BodyguardConfig _config;
        private readonly List<Bodyguard> _bodyguards = new List<Bodyguard>();
        
        public BodyguardsManager(BodyguardConfig config)
        {
            _config = config;
            _started = false;
        }
        
        public async Task SpawnBodyguard()
        {
            Debug.WriteLine("[Bodyguard] Try spawn bodyguard");
            
            var owner = Game.Player.Character;
            var bodyGuardHash = PedHash.ChemSec01SMM;
            var bodyGuardHashUint = (uint) bodyGuardHash;
            var success = await ModelLoader.LoadModel(bodyGuardHashUint);

            if (success)
            {
                var spawnPoint = owner.Position + owner.ForwardVector * 2;
                var bodyguardPed = await World.CreatePed(bodyGuardHash, spawnPoint);
                var bodyGuard = new Bodyguard(bodyguardPed, owner);
                AddBodyguard(bodyGuard);
                Debug.WriteLine("[Bodyguard] Bodyguard spawn finished");    
            }
        }

        private void AddBodyguard(Bodyguard guard)
        {
            if (_bodyguards.Count == 0)
            {
                _started = true;
            }
            
            _bodyguards.Add(guard);
        }

        public void RemoveBodyguard()
        {
            //Remove logic
            
            if (_bodyguards.Count == 0)
            {
                _started = false;
            }
        }

        public void Update()
        {
            if (_started)
            {
                foreach (var guard in _bodyguards)
                {
                    guard.Update();
                }
            }
        }
    }
}