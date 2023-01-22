using CitizenFX.Core;
using CitizenFX.Core.Native;
using Client.Ext;

// ReSharper disable once CheckNamespace
namespace Client.States
{
    public class SpawnState : IState
    {
        private const int DefaultCombatBehaviour = 2;
        private const int DefaultAmmoCount = 500;
        
        private readonly StateContext _context;

        public SpawnState(StateContext context)
        {
            Debug.WriteLine("[SpawnState] Start");
            
            _context = context;
        }
        
        public void Update()
        {
            var bodyguardPed = _context.BodyguardPed;
            API.SetPedCombatAbility(bodyguardPed.Handle, DefaultCombatBehaviour);
            API.GiveWeaponToPed(bodyguardPed.Handle, (uint)WeaponHash.AssaultRifleMk2, DefaultAmmoCount, false, true);
            bodyguardPed.PlayAmbientSpeech("GENERIC_HI");
            
            //вычисляем позицию гварда
            //закрепляем за гвардом позицию
            //отправляем в FollowState
            
            
            var player = _context.OwnerPed;
            var playerPos = player.Position;
            var guardPos = _context.BodyguardPed.Position;
            var distanceToPlayer = guardPos.DistanceTo(playerPos);
            var speed = 10;

            if (distanceToPlayer > 3)
            {
                API.TaskGoToEntity(_context.BodyguardPed.Handle, player.Handle, -1, 2.0f, speed, 1073741824, 0);    
            }
            else
            {
                var states = _context.BotStates;
                states.Pop();
                states.Push(new FollowState(_context));
            }
        }
    }
}