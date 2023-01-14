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
            
            API.SetPedCombatAbility(bodyguardPed.Handle, DefaultCombatBehaviour);
            Debug.WriteLine("[Bodyguard] SetPedCombatAbility");
            
            API.GiveWeaponToPed(bodyguardPed.Handle, (uint)WeaponHash.AssaultRifleMk2, DefaultAmmoCount, false, true);
            Debug.WriteLine("[Bodyguard] GiveWeaponToPed");
            
            bodyguardPed.PlayAmbientSpeech("GENERIC_HI");
            Debug.WriteLine("[Bodyguard] PlayAmbientSpeech");

            var context = new StateContext(_botStates, bodyguardPed, ownerPed);
            _botStates.Push(new FollowState(context));
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