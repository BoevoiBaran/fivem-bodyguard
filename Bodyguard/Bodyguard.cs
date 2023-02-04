using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.States;

// ReSharper disable once CheckNamespace
namespace Client
{
    public class Bodyguard
    {
        private const int DefaultCombatBehaviour = 2;
        private const int DefaultAmmoCount = 500;
        
        public Ped GuardPed => Context.BodyguardPed;
        
        public readonly StateContext Context;
        private readonly Stack<IState> _botStates = new Stack<IState>();
        
        public Bodyguard(StateContext context, uint bodyguardGroupHash)
        {
            Context = context;
            Context.SetupStates(_botStates);

            var pedHash = context.BodyguardPed.Handle;
            API.SetPedAsGroupMember(pedHash, (int) bodyguardGroupHash);
            API.SetPedCombatAbility(pedHash, DefaultCombatBehaviour);
            API.GiveWeaponToPed(pedHash, (uint)WeaponHash.AssaultRifleMk2, DefaultAmmoCount, false, true);
        }

        public void PushInitState(IState state)
        {
            _botStates.Push(state);
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