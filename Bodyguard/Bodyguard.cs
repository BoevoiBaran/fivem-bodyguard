using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.States;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class Bodyguard
    {
        public readonly Ped BodyguardPed;
        public readonly Ped OwnerPed;
        
        private readonly Stack<IState> _botStates = new Stack<IState>();
        
        public Bodyguard(Ped bodyguardPed, Ped owner)
        {
            BodyguardPed = bodyguardPed;
            OwnerPed = owner;
            
            Initialize(bodyguardPed, owner);
        }

        private void Initialize(Ped bodyguardPed, Ped ownerPed)
        {
            Debug.WriteLine("[Bodyguard] Start initialize");

            var context = new StateContext(_botStates, bodyguardPed, ownerPed);
            _botStates.Push(new SpawnState(context));
        }
        
        public void Update()
        {
            if (_botStates.Count != 0)
            {
                _botStates.Peek()?.Update();    
            }
        }
    }
}